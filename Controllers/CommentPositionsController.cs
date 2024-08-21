using Microsoft.AspNetCore.Mvc;
using App.Interfaces;
using App.Models;

namespace App.Controllers
{
    [ApiController]
    [Route("api/CommentPositions")]
    public class CommentPositionController : ControllerBase
    {
        private readonly ICommentPositionService _commentPositionService;

        public CommentPositionController(ICommentPositionService commentPositionService)
        {
            _commentPositionService = commentPositionService;
        }

        [HttpGet("{kommentarId}")]
        public async Task<ActionResult<CommentPosition>> GetPositionByKommentarId(int kommentarId)
        {
            var position = await _commentPositionService.GetPositionByKommentarIdAsync(kommentarId);
            if (position == null)
            {
                return NotFound();
            }
            return Ok(position);
        }

        [HttpPost]
        public async Task<ActionResult<CommentPosition>> AddOrUpdatePosition([FromBody] CommentPosition position)
        {
            var updatedPosition = await _commentPositionService.AddOrUpdatePositionAsync(position);
            return Ok(updatedPosition);
        }

        [HttpDelete("{kommentarId}")]
        public async Task<IActionResult> DeletePosition(int kommentarId)
        {
            var success = await _commentPositionService.DeletePositionByKommentarIdAsync(kommentarId);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
