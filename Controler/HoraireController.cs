// Fichier : Controllers/HoraireController.cs
using ASPPorcelette.API.DTOs.Horaire;
using ASPPorcelette.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoraireController : ControllerBase
    {
        private readonly IHoraireService _horaireService;

        public HoraireController(IHoraireService horaireService)
        {
            _horaireService = horaireService;
        }

        /// <summary>
        /// Récupère la liste de tous les horaires de cours.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HoraireDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var horaires = await _horaireService.GetAllHorairesAsync();
            return Ok(horaires);
        }

        /// <summary>
        /// Récupère un horaire spécifique par son ID.
        /// </summary>
        /// <param name="id">ID de l'horaire.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(HoraireDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var horaire = await _horaireService.GetHoraireByIdAsync(id);

            if (horaire == null)
            {
                return NotFound();
            }

            return Ok(horaire);
        }
    }
}