using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Models;
using App.Interfaces;
using App.Data;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using App.Services;
using System.IO;
using File = App.Models.FileModel;

namespace App.Controllers
{
    [Route("api/Forschungsfragen")]
    [ApiController]
    public class ForschungsfrageController : ControllerBase
    {
        private readonly IForschungsfrageService _forschungsfrageService;
        private readonly IAzureBlobStorageService _azureBlobStorageService;


        public ForschungsfrageController(
        IForschungsfrageService forschungsfrageService,
        IAzureBlobStorageService azureBlobStorageService
            

            )
        {
            _forschungsfrageService = forschungsfrageService;
            _azureBlobStorageService = azureBlobStorageService;

        }

        [HttpGet]
        public ActionResult<IEnumerable<Forschungsfrage>> GetAllForschungsfragen() => Ok(_forschungsfrageService.GetAllForschungsfragen());


        [HttpGet("{id}")]
        public IActionResult GetForschungsfrageById(int id)
        {
            var forschungsfrage = _forschungsfrageService.GetForschungsfrageById(id);
            if (forschungsfrage == null)
            {
                return NotFound();
            }
            return Ok(forschungsfrage);
        }
        [HttpGet("latest")]
        public IActionResult GetLatestForschungsfrage()
        {
            var forschungsfrage = _forschungsfrageService.GetLatestForschungsfrage();
            if (forschungsfrage == null)
            {
                return NotFound();
            }
            return Ok(forschungsfrage);
        }

        [HttpPost]
        public async Task<ActionResult<Forschungsfrage>> CreateForschungsfrage([FromForm] Forschungsfrage forschungsfrage, [FromForm] IFormFile image)
        {
            var createdForschungsfrage = await _forschungsfrageService.CreateForschungsfrage(forschungsfrage, image);
            if (createdForschungsfrage == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetForschungsfrageById), new { id = createdForschungsfrage.ID }, createdForschungsfrage);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateForschungsfrage(int id, [FromBody] Forschungsfrage forschungsfrage)
        {
            if (forschungsfrage == null || forschungsfrage.ID != id)
            {
                return BadRequest();
            }

            var existingForschungsfrage = _forschungsfrageService.GetForschungsfrageById(id);
            if (existingForschungsfrage == null)
            {
                return NotFound();
            }

            _forschungsfrageService.UpdateForschungsfrage(id, forschungsfrage);
            return NoContent();
        }
        [HttpPut("{id}/backgroundColor")]
        public IActionResult UpdateBackgroundColor(int id, [FromBody] string backgroundColor)
        {
            var updated = _forschungsfrageService.UpdateBackgroundColor(id, backgroundColor);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteForschungsfrage(int id)
        {
            var forschungsfrage = _forschungsfrageService.GetForschungsfrageById(id);
            if (forschungsfrage == null)
            {
                return NotFound();
            }

            _forschungsfrageService.DeleteForschungsfrage(id);
            return NoContent();
        }
    }
}
