using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Models;
using App.Interfaces;
using App.Data;
using Microsoft.EntityFrameworkCore;
using App.Services;
using App.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace App.Controllers
{
    [ApiController]
    [Route("api/Kommentare")]
    public class KommentarController : ControllerBase
    {
        private readonly IKommentarService _kommentarService;
        private readonly IHubContext<NotificationHub> _hubContext;


        public KommentarController(
            IKommentarService kommentarService,
            IHubContext<NotificationHub> hubContext)
        {
            _kommentarService = kommentarService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<ActionResult<Kommentar>> PostKommentar(Kommentar kommentar)
        {
            var createdKommentar = await _kommentarService.AddKommentar(kommentar);
            if (createdKommentar == null)
            {
                return BadRequest();
            }
            await _hubContext.Clients.All.SendAsync("ReceiveCommentUpdate", "added", createdKommentar);

            return CreatedAtAction(nameof(GetKommentar), new { id = createdKommentar.Id }, createdKommentar);
        }

        [HttpPost("uploadmedia")]
public async Task<IActionResult> UploadMedia([FromForm] IFormFile media, [FromForm] int forschungsfrageId)
{
    // Upload the media and get the URL
    var mediaUrl = await _kommentarService.AddMedia(media, forschungsfrageId);
    if (string.IsNullOrEmpty(mediaUrl))
    {
        return BadRequest("Error uploading media.");
    }

    // Create a new FileModel instance
    var fileModel = new FileModel
    {
        FileName = media.FileName,             // Use the original file name
        FileType = media.ContentType,          // For example: "image/png" or "video/mp4"
        FileSize = media.Length,
        UploadDate = DateTime.UtcNow,
        BlobStorageUri = mediaUrl,
        ForschungsfrageId = forschungsfrageId,
        IsDeleted = false
    };

    // (Optional) Save fileModel to the database if needed
    // await _kommentarService.AddFile(fileModel);

    // Notify all connected clients in real time with the complete object.
    await _hubContext.Clients.All.SendAsync("ReceiveMediaUpdate", fileModel);

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
            await _hubContext.Clients.All.SendAsync("ReceiveCommentDeleteUpdate", id);
            return NoContent();
        }

        [HttpPut("edit-commentar/{id}")]
        public async Task<IActionResult> EditKommentar(int id, [FromBody] Kommentar kommentar)
        {
            if (id != kommentar.Id)
            {
                return BadRequest("Kommentar ID mismatch.");
            }

            var updatedKommentar = await _kommentarService.EditKommentar(id, kommentar);
            if (updatedKommentar == null)
            {
                return NotFound();
            }

            await _hubContext.Clients.All.SendAsync("ReceiveEditKommentarUpdate", updatedKommentar);

            return Ok(updatedKommentar);
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
            await _hubContext.Clients.All.SendAsync("ReceiveMediaDeleteUpdate", fileName);

            return Ok(new { message = "File deleted successfully." });
        }


    }
}
