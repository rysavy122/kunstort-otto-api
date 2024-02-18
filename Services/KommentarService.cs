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
