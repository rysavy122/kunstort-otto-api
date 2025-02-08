using Microsoft.AspNetCore.Mvc;
using App.Interfaces;
using App.Models;
using Microsoft.AspNetCore.SignalR;
using App.Hubs;

namespace App.Controllers
{
    [ApiController]
    [Route("api/CommentPositions")]
    public class CommentPositionController : ControllerBase
    {
        private readonly ICommentPositionService _commentPositionService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CommentPositionController(
            ICommentPositionService commentPositionService,         
            IHubContext<NotificationHub> hubContext
)
        {
            _commentPositionService = commentPositionService;
            _hubContext = hubContext;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentPosition>>> GetAllPositions()
        {
            var positions = await _commentPositionService.GetAllPositionsAsync();
            return Ok(positions);
        }

        [HttpPost]
        public async Task<ActionResult<CommentPosition>> AddOrUpdatePosition([FromBody] CommentPosition position)
        {
            var updatedPosition = await _commentPositionService.AddOrUpdatePositionAsync(position);
            
            await _hubContext.Clients.All.SendAsync("ReceiveCommentPositionUpdate", updatedPosition);

            return Ok(updatedPosition);
        }
    }
}
