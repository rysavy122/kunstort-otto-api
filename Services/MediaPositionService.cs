using App.Data;
using App.Interfaces;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class MediaPositionService : IMediaPositionService
    {
        private readonly OttoDbContext _context;

        public MediaPositionService(OttoDbContext context)
        {
            _context = context;
        }

        public async Task<MediaPosition?> GetPositionByFileModelIdAsync(int fileModelId)
        {
            return await _context.MediaPositions
                .FirstOrDefaultAsync(mp => mp.FileModelId == fileModelId);
        }

        public async Task<MediaPosition> AddOrUpdatePositionAsync(MediaPosition position)
        {
            var existingPosition = await GetPositionByFileModelIdAsync(position.FileModelId);
            if (existingPosition != null)
            {
                existingPosition.XPosition = position.XPosition;
                existingPosition.YPosition = position.YPosition;
                existingPosition.BorderColor = position.BorderColor;
                _context.MediaPositions.Update(existingPosition);
            }
            else
            {
                await _context.MediaPositions.AddAsync(position);
            }
            await _context.SaveChangesAsync();
            return position;
        }
    }
}
