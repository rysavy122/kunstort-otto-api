namespace App.Interfaces
{
    public interface ICommentPositionService
    {
        Task<CommentPosition?> GetPositionByKommentarIdAsync(int kommentarId);
        Task<CommentPosition> AddOrUpdatePositionAsync(CommentPosition position);
        Task<bool> DeletePositionByKommentarIdAsync(int kommentarId);
    }
}
