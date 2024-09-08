using App.Models;

namespace App.Interfaces
{
    public interface IMediaPositionService
    {
        Task<MediaPosition?> GetPositionByFileModelIdAsync(int fileModelId);
        Task<MediaPosition> AddOrUpdatePositionAsync(MediaPosition position);
        Task<IEnumerable<MediaPosition>> GetAllPositionsAsync();

    }
}
