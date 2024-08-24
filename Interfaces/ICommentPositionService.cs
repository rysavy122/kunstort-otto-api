using App.Models;
using System.Threading.Tasks;

namespace App.Interfaces
{
    public interface ICommentPositionService
    {
        Task<CommentPosition?> GetPositionByKommentarIdAsync(int kommentarId);
        Task<CommentPosition> AddOrUpdatePositionAsync(CommentPosition position);
    }
}
