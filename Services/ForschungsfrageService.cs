using System;
using App.Data;
using App.Interfaces;
using App.Models;

namespace App.Services
{
    public class ForschungsfrageService : IForschungsfrageService
    {

        private readonly OttoDbContext? _context;
        private readonly IAzureBlobStorageService? _azureBlobStorageService;


        public ForschungsfrageService(OttoDbContext context, IAzureBlobStorageService azureBlobStorageService)
        {
            _context = context;
            _azureBlobStorageService = azureBlobStorageService;

        }

        // CREATE FORSCHUNGSFRAGE
        public async Task<Forschungsfrage> CreateForschungsfrage(Forschungsfrage forschungsfrage, IFormFile image)
        {
            if (_context == null || forschungsfrage == null)
            {
                return null;
            }

            if (image != null && image.Length > 0)
            {
                string imageUrl = await _azureBlobStorageService.UploadImageToBlobAsync(image);
                forschungsfrage.ImagePath = imageUrl;
            }

            _context.Forschungsfragen.Add(forschungsfrage);
            await _context.SaveChangesAsync();
            return forschungsfrage;
        }

        //GET ALL FORSCHUNGSFRAGEN
        public IEnumerable<Forschungsfrage> GetAllForschungsfragen()
        {
            return _context?.Forschungsfragen.ToList() ?? Enumerable.Empty<Forschungsfrage>();
        }


        // GET SINGLE FORSCHUNGSFRAGE
        public Forschungsfrage? GetForschungsfrageById(int id)
        {
            return _context?.Forschungsfragen.FirstOrDefault(m => m.Id == id);
        }
        //GET LATEST FORSCHUNGSFRAGE
        public Forschungsfrage GetLatestForschungsfrage()
        {
            return _context?.Forschungsfragen.OrderByDescending(m => m.Id).FirstOrDefault();
        }

        // EDIT FORSCHUNGSFRAGE
        public Forschungsfrage UpdateForschungsfrage(int id, Forschungsfrage forschungsfrage)
        {
            var existingForschungsfrage = _context?.Forschungsfragen.FirstOrDefault(f => f.Id == id);
            if (existingForschungsfrage != null)
            {
                existingForschungsfrage.Title = forschungsfrage.Title;
                _context?.SaveChanges();
            }
            return existingForschungsfrage;
        }


        // DELETE FORSCHUNGSFRAGE
        public void DeleteForschungsfrage(int id)
        {
            var forschungsfrage = _context?.Forschungsfragen.FirstOrDefault(m => m.Id == id);
            if (forschungsfrage != null)
            {
                _context.Forschungsfragen.Remove(forschungsfrage);
                _context.SaveChanges();
            }
        }
    }
}
