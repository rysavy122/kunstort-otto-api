using App.Models.DTOs;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Interfaces;


namespace App.Controllers
{
    [ApiController]
    [Route("api/plakate")]
    public class PlakatController : ControllerBase
    {
        private readonly IPlakatService _plakatService;

        public PlakatController(IPlakatService plakatService)
        {
            _plakatService = plakatService;
        }

        [HttpGet("{plakatId}")]
        public async Task<IActionResult> GetPlakat(int plakatId)
        {
            var plakat = await _plakatService.GetPlakatById(plakatId);
            if (plakat == null) return NotFound();

            return Ok(plakat);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlakat([FromBody] PlakatDto plakatDto)
        {
            var createdPlakat = await _plakatService.CreatePlakat(plakatDto);
            return CreatedAtAction(nameof(GetPlakat), new { plakatId = createdPlakat.Id }, createdPlakat);
        }

        [HttpPut("{plakatId}")]
        public async Task<IActionResult> UpdatePlakat(int plakatId, [FromBody] string drawingJson)
        {
            var success = await _plakatService.UpdatePlakat(plakatId, drawingJson);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpDelete("{plakatId}")]
        public async Task<IActionResult> DeletePlakat(int plakatId)
        {
            var success = await _plakatService.DeletePlakat(plakatId);
            if (!success) return NotFound();

            return NoContent();
        }

        [HttpPost("{plakatId}/stickers")]
        public async Task<IActionResult> AddSticker(int plakatId, [FromBody] StickerDto stickerDto)
        {
            var success = await _plakatService.AddSticker(plakatId, stickerDto);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}
