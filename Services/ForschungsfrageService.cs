using System;
using App.Data;
using App.Interfaces;
using App.Models;

namespace App.Services
{
    public class ForschungsfrageService : IForschungsfrageService
    {

        private readonly OttoDbContext _context;
        private readonly IAzureBlobStorageService _azureBlobStorageService;


        public ForschungsfrageService(OttoDbContext context, IAzureBlobStorageService azureBlobStorageService)
        {
            _context = context;
            _azureBlobStorageService = azureBlobStorageService;

        }
        public async Task<Forschungsfrage> CreateForschungsfrage(Forschungsfrage forschungsfrage, IFormFile image)
        {
            if (_context == null || forschungsfrage == null)
            {
                return null;
            }

            var allComments = _context.Kommentare.ToList();
            _context.Kommentare.RemoveRange(allComments);
            await _context.SaveChangesAsync();

            if (image != null && image.Length > 0)
            {
                var (imageUrl, fileInfo) = await _azureBlobStorageService.UploadImageToBlobAsync(image);
                forschungsfrage.ImagePath = imageUrl;

                _context.Files.Add(fileInfo);
                await _context.SaveChangesAsync();
            }

            forschungsfrage.BackgroundColor = forschungsfrage.BackgroundColor ?? "#FFFFFF";

            _context.Forschungsfragen.Add(forschungsfrage);
            await _context.SaveChangesAsync();
            return forschungsfrage;
        }
        public IEnumerable<Forschungsfrage> GetAllForschungsfragen()
        {
            return _context.Forschungsfragen.ToList() ?? Enumerable.Empty<Forschungsfrage>();
        }

        public Forschungsfrage GetForschungsfrageById(int id)
        {
            return _context.Forschungsfragen.FirstOrDefault(m => m.ID == id);
        }
        public Forschungsfrage GetLatestForschungsfrage()
        {
            return _context.Forschungsfragen.OrderByDescending(m => m.ID).FirstOrDefault();
        }

        public Forschungsfrage UpdateForschungsfrage(int id, Forschungsfrage forschungsfrage)
        {
            var existingForschungsfrage = _context.Forschungsfragen.FirstOrDefault(f => f.ID == id);
            if (existingForschungsfrage != null)
            {
                existingForschungsfrage.Title = forschungsfrage.Title;
                existingForschungsfrage.BackgroundColor = forschungsfrage.BackgroundColor ?? existingForschungsfrage.BackgroundColor;

                _context.SaveChanges();
            }
            return existingForschungsfrage;
        }
        public Forschungsfrage UpdateBackgroundColor(int id, string backgroundColor)
        {
            var existingForschungsfrage = _context.Forschungsfragen.FirstOrDefault(f => f.ID == id);
            if (existingForschungsfrage != null)
            {
                existingForschungsfrage.BackgroundColor = backgroundColor;
                _context.SaveChanges();
            }
            return existingForschungsfrage;
        }

        public void DeleteForschungsfrage(int id)
        {
            var forschungsfrage = _context.Forschungsfragen.FirstOrDefault(m => m.ID == id);
            if (forschungsfrage != null)
            {
                _context.Forschungsfragen.Remove(forschungsfrage);
                _context.SaveChanges();
            }
        }
    }
}
