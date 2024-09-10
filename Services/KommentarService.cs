using System;
using App.Data;
using App.Models;
using App.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.Logging;

namespace App.Services
{
    public class KommentarService : IKommentarService
    {
        private readonly OttoDbContext _context;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly ILogger _logger;


        public KommentarService(OttoDbContext context, IAzureBlobStorageService azureBlobStorageService, ILogger<KommentarService> logger)
        {
            _context = context;
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;


        }


        // ADD MEDIA
        public async Task<string> AddMedia(IFormFile media, int forschungsfrageId)
        {
            _logger.LogInformation($"Received forschungsfrageId: {forschungsfrageId}");

            if (_context == null || media == null || media.Length == 0)
            {
                return null;
            }

            string mediaType = media.ContentType.Split('/')[0].ToLower();
            (string Uri, FileModel FileInfo) uploadResult;

            switch (mediaType)
            {
                case "image":
                    uploadResult = await _azureBlobStorageService.UploadImageToBlobAsync(media);
                    break;
                case "video":
                    uploadResult = await _azureBlobStorageService.UploadVideoToBlobAsync(media);
                    break;
                case "audio":
                    uploadResult = await _azureBlobStorageService.UploadAudioToBlobAsync(media);
                    break;
                default:
                    return null;
            }

            if (string.IsNullOrEmpty(uploadResult.Uri))
            {
                return null;
            }

            uploadResult.FileInfo.ForschungsfrageId = forschungsfrageId;
            _context.Files.Add(uploadResult.FileInfo);

            await _context.SaveChangesAsync();

            return uploadResult.Uri;
        }



        // DELETE MEDIA
        public async Task<bool> DeleteMedia(string fileName)
        {
            if (_context == null || string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var file = _context.Files.FirstOrDefault(f => f.FileName == fileName && !f.IsDeleted);
            if (file != null)
            {
                file.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            var success = await _azureBlobStorageService.DeleteMediaAsync(file.FileName);
            if (!success)
            {
                return false;
            }

            _context.Files.Remove(file);
            await _context.SaveChangesAsync();

            return true;
        }


        // CREATE A COMMENT
        public async Task<Kommentar> AddKommentar(Kommentar kommentar)
        {
            if (_context == null || kommentar == null)
            {
                return null;
            }

            if (kommentar.ParentKommentarId != null)
            {
                var parentKommentar = await _context.Kommentare.FindAsync(kommentar.ParentKommentarId);
                if (parentKommentar == null)
                {
                    return null;
                }
            }

            _context.Kommentare.Add(kommentar);
            await _context.SaveChangesAsync();
            return kommentar;
        }

        // LOAD REPLIES
        private async Task LoadReplies(Kommentar kommentar)
        {
            if (_context == null || kommentar == null) return;

            kommentar.Replies = await _context.Kommentare
                .Where(k => k.ParentKommentarId == kommentar.Id)
                .ToListAsync();

            foreach (var reply in kommentar.Replies)
            {
                await LoadReplies(reply);
            }
        }


        // GET ALL COMMENTS
        public async Task<IEnumerable<Kommentar>> GetAllKommentare()
        {
            if (_context == null)
            {
                return Enumerable.Empty<Kommentar>();
            }

            var topLevelKommentare = await _context.Kommentare
                .Where(k => k.ParentKommentarId == null)
                .ToListAsync();

            foreach (var kommentar in topLevelKommentare)
            {
                await LoadReplies(kommentar);
            }

            return topLevelKommentare;
        }

        // GET ALL MEDIA OF FORSCHUNGSFRAGE
        public async Task<IEnumerable<FileModel>> GetMediaByForschungsfrageId(int forschungsfrageId)
        {
            if (_context == null)
            {
                return Enumerable.Empty<FileModel>();
            }

            return await _context.Files
                    .Where(f => f.ForschungsfrageId == forschungsfrageId && !f.IsDeleted)
                    .ToListAsync();
        }


        // GET SINGLE MEDIA BY ID 

        // GETS SINGLE COMMENT BY ID
        public async Task<Kommentar> GetKommentarById(int id)
        {
            return await _context.Kommentare.FirstOrDefaultAsync(k => k.Id == id);
        }

        //DELETE A COMMENT
        public async Task<bool> DeleteKommentar(int id)
        {
            var kommentar = await _context.Kommentare.FindAsync(id);
            if (kommentar == null) return false;

            if (kommentar.ParentKommentarId == null)
            {
                // If it's a parent comment, remove all its replies
                var replies = await _context.Kommentare.Where(k => k.ParentKommentarId == id).ToListAsync();
                _context.Kommentare.RemoveRange(replies);
            }

            _context.Kommentare.Remove(kommentar);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
