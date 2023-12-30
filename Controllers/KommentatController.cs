﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Interfaces;
using App.Models;

namespace App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return CreatedAtAction(nameof(GetKommentar), new { id = createdKommentar.Id }, createdKommentar);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kommentar>>> GetAllKommentare()
        {
            var kommentare = await _kommentarService.GetAllKommentare();
            return Ok(kommentare);
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

    }
}
