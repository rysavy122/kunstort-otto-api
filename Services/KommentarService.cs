using System;
using App.Data;
using App.Models;
using App.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class KommentarService : IKommentarService
    {
        private readonly OttoDbContext? _context;

        public KommentarService(OttoDbContext context)
        {
            _context = context;
        }

        public async Task<Kommentar> AddKommentar(Kommentar kommentar)
        {
            if (_context == null || kommentar == null)
            {
                return null;
            }
            _context.Kommentare.Add(kommentar);
            await _context.SaveChangesAsync();
            return kommentar;
        }

        public async Task<IEnumerable<Kommentar>> GetAllKommentare()
        {
            return await _context?.Kommentare.ToListAsync() ?? Enumerable.Empty<Kommentar>();

        }
        public async Task<Kommentar> GetKommentarById(int id)
        {
            return await _context.Kommentare.FirstOrDefaultAsync(k => k.Id == id);
        }
    }
}
