using App.Data;
using App.Models;
using App.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using App.Interfaces;  // Use the interface from App.Interfaces
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{

    public class PlakatService : IPlakatService
    {
        private readonly OttoDbContext _context;

        public PlakatService(OttoDbContext context)
        {
            _context = context;
        }

        public async Task<PlakatDto> GetPlakatById(int plakatId)
        {
            var plakat = await _context.Plakats
                .Include(p => p.Stickers)
                .FirstOrDefaultAsync(p => p.Id == plakatId);

            if (plakat == null)
                return null;

            return new PlakatDto
            {
                Id = plakat.Id,
                Title = plakat.Title,
                DrawingJson = plakat.DrawingJson,
                Stickers = plakat.Stickers.Select(s => new StickerDto
                {
                    Id = s.Id,
                    FileName = s.FileName,
                    FileType = s.FileType,
                    BlobStorageUri = s.BlobStorageUri,
                    PlakatId = s.PlakatId
                }).ToList()
            };
        }

        public async Task<PlakatDto> CreatePlakat(PlakatDto plakatDto)
        {
            var plakat = new Plakat
            {
                Title = plakatDto.Title,
                DrawingJson = plakatDto.DrawingJson
            };

            _context.Plakats.Add(plakat);
            await _context.SaveChangesAsync();

            return new PlakatDto
            {
                Id = plakat.Id,
                Title = plakat.Title,
                DrawingJson = plakat.DrawingJson,
                Stickers = new List<StickerDto>()
            };
        }

        public async Task<bool> UpdatePlakat(int plakatId, string drawingJson)
        {
            var plakat = await _context.Plakats.FirstOrDefaultAsync(p => p.Id == plakatId);
            if (plakat == null) return false;

            plakat.DrawingJson = drawingJson;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePlakat(int plakatId)
        {
            var plakat = await _context.Plakats.FirstOrDefaultAsync(p => p.Id == plakatId);
            if (plakat == null) return false;

            _context.Plakats.Remove(plakat);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSticker(int plakatId, StickerDto stickerDto)
        {
            var sticker = new Sticker
            {
                FileName = stickerDto.FileName,
                FileType = stickerDto.FileType,
                BlobStorageUri = stickerDto.BlobStorageUri,
                PlakatId = plakatId
            };

            _context.Stickers.Add(sticker);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
