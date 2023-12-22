using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Models;
using App.Interfaces;
using App.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers
{
    [Route("api/Forschungsfragen")]
    [ApiController]
    public class ForschungsfrageController : ControllerBase
    {
        private readonly IForschungsfrageService _forschungsfrageService;

        public ForschungsfrageController(
            IForschungsfrageService forschungsfrageService
            )
        {
            _forschungsfrageService = forschungsfrageService;
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

        [HttpPost]
        public ActionResult<Forschungsfrage> CreateForschungsfrage(Forschungsfrage forschungsfrage)
        {
            var createdForschungsfrage = _forschungsfrageService.CreateForschungsfrage(forschungsfrage);
            if (createdForschungsfrage == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetForschungsfrageById), new { id = createdForschungsfrage.Id }, createdForschungsfrage);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateForschungsfrage(int id, [FromBody] Forschungsfrage forschungsfrage)
        {
            if (forschungsfrage == null || forschungsfrage.Id != id)
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
