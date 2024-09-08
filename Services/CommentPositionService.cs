using App.Data;
using App.Interfaces;
using App.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace App.Services
{
    public class CommentPositionService : ICommentPositionService
    {
        private readonly OttoDbContext _context;

        public CommentPositionService(OttoDbContext context)
        {
            _context = context;
        }

        public async Task<CommentPosition?> GetPositionByKommentarIdAsync(int kommentarId)
        {
            return await _context.CommentPositions
                .FirstOrDefaultAsync(cp => cp.KommentarId == kommentarId);
        }

        public async Task<IEnumerable<CommentPosition>> GetAllPositionsAsync()
        {
            return await _context.CommentPositions.ToListAsync();
        }


        public async Task<CommentPosition> AddOrUpdatePositionAsync(CommentPosition position)
        {
            var existingPosition = await GetPositionByKommentarIdAsync(position.KommentarId);
            if (existingPosition != null)
            {
                existingPosition.XPosition = position.XPosition;
                existingPosition.YPosition = position.YPosition;
                existingPosition.BorderColor = position.BorderColor;
                _context.CommentPositions.Update(existingPosition);
            }
            else
            {
                await _context.CommentPositions.AddAsync(position);
            }
            await _context.SaveChangesAsync();
            return position;
        }

    }
}
