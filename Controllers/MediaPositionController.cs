using Microsoft.AspNetCore.Mvc;
using App.Interfaces;
using App.Models;
using Microsoft.AspNetCore.SignalR;
using App.Hubs;

namespace App.Controllers
{
  [ApiController]
  [Route("api/MediaPositions")]
  public class MediaPositionController : ControllerBase
  {
    private readonly IMediaPositionService _mediaPositionService;
    private readonly IHubContext<NotificationHub> _hubContext;


    public MediaPositionController(
      IMediaPositionService mediaPositionService,
      IHubContext<NotificationHub> hubContext
)
    {
      _mediaPositionService = mediaPositionService;
      _hubContext = hubContext;
    }

    [HttpGet("{fileModelId}")]
    public async Task<ActionResult<MediaPosition>> GetPositionByFileModelId(int fileModelId)
    {
      var position = await _mediaPositionService.GetPositionByFileModelIdAsync(fileModelId);
      if (position == null)
      {
        return NotFound();
      }
      return Ok(position);
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MediaPosition>>> GetAllPositions()
    {
      var positions = await _mediaPositionService.GetAllPositionsAsync();
      return Ok(positions);
    }

    [HttpPost]
    public async Task<ActionResult<MediaPosition>> AddOrUpdatePosition([FromBody] MediaPosition position)
    {
      var updatedPosition = await _mediaPositionService.AddOrUpdatePositionAsync(position);
      await _hubContext.Clients.All.SendAsync("ReceiveMediatPositionUpdate", updatedPosition);

      return Ok(updatedPosition);
    }
    /*         [HttpPut("media/{fileModelId}/position")]
            public async Task<IActionResult> UpdateMediaPosition(int fileModelId, [FromBody] MediaPosition position)
            {
                if (fileModelId != position.FileModelId)
                {
                    return BadRequest();
                }

                var updatedPosition = await _mediaPositionService.AddOrUpdatePositionAsync(position);
                if (updatedPosition == null)
                {
                    return NotFound();
                }

                return Ok(updatedPosition);
            } */
  }
}
