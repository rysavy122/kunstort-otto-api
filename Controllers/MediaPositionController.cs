using Microsoft.AspNetCore.Mvc;
using App.Interfaces;
using App.Models;

namespace App.Controllers
{
    [ApiController]
    [Route("api/MediaPositions")]
    public class MediaPositionController : ControllerBase
    {
        private readonly IMediaPositionService _mediaPositionService;

        public MediaPositionController(IMediaPositionService mediaPositionService)
        {
            _mediaPositionService = mediaPositionService;
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

        [HttpPost]
        public async Task<ActionResult<MediaPosition>> AddOrUpdatePosition([FromBody] MediaPosition position)
        {
            var updatedPosition = await _mediaPositionService.AddOrUpdatePositionAsync(position);
            return Ok(updatedPosition);
        }
    }
}
