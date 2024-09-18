using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Models;
using App.Interfaces;
using App.Data;
using Microsoft.EntityFrameworkCore;
using App.Services;

namespace App.Controllers
{
    [ApiController]
    [Route("api/Kommentare")]
    public class KommentarController : ControllerBase
    {
        private readonly IKommentarService _kommentarService;

        public KommentarController(IKommentarService kommentarService)
        {
            _kommentarService = kommentarService;
        }

        [HttpPost]
        public async Task<ActionResult<Kommentar>> PostKommentar(Kommentar kommentar)
        {
            var createdKommentar = await _kommentarService.AddKommentar(kommentar);
            if (createdKommentar == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetKommentar), new { id = createdKommentar.Id }, createdKommentar);
        }

        [HttpPost("uploadmedia")]
        public async Task<IActionResult> UploadMedia([FromForm] IFormFile media, [FromForm] int forschungsfrageId)
        {
            var mediaUrl = await _kommentarService.AddMedia(media, forschungsfrageId);
            if (string.IsNullOrEmpty(mediaUrl))
            {
                return BadRequest("Error uploading media.");
            }

            return Ok(new { Url = mediaUrl });
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kommentar>>> GetAllKommentare()
        {
            var kommentare = await _kommentarService.GetAllKommentare();
            return Ok(kommentare);
        }
        [HttpGet("media/{forschungsfrageId}")]
        public async Task<ActionResult<IEnumerable<FileModel>>> GetMedia(int forschungsfrageId)
        {
            var mediaFiles = await _kommentarService.GetMediaByForschungsfrageId(forschungsfrageId);
            if (mediaFiles == null || !mediaFiles.Any())
            {
                return NotFound();
            }

            return Ok(mediaFiles);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Kommentar>> GetKommentar(int id)
        {
            var kommentar = await _kommentarService.GetKommentarById(id);
            if (kommentar == null)
            {
                return NotFound();
            }
            return kommentar;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKommentar(int id)
        {
            var success = await _kommentarService.DeleteKommentar(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("DeleteMedia/{fileName}")]
        public async Task<IActionResult> DeleteMedia(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is required.");
            }

            var success = await _kommentarService.DeleteMedia(fileName);
            if (!success)
            {
                return NotFound("File not found or could not be deleted.");
            }

        return Ok(new { message = "File deleted successfully." });
        }


    }
}
