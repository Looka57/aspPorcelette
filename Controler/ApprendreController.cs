using ASPPorcelette.API.DTOs.Apprendre;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // S'assurer que StatusCodes est disponible

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApprendreController : ControllerBase
    {
        private readonly IApprendreService _apprendreService;
        private readonly IMapper _mapper;

        public ApprendreController(IApprendreService apprendreService, IMapper mapper)
        {
            _apprendreService = apprendreService;
            _mapper = mapper;
        }

        // GET: api/Apprendre
        // Récupère toutes les inscriptions avec les détails des relations (Adherent, Discipline)
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ApprendreDto>>> GetAllInscriptions()
        {
            var inscriptions = await _apprendreService.GetAllInscriptionsAsync();
            var inscriptionsDto = _mapper.Map<IEnumerable<ApprendreDto>>(inscriptions);
            return Ok(inscriptionsDto);
        }

        // GET: api/Apprendre/{adherentId}/{disciplineId}
        // Récupère une inscription spécifique par la clé composite
        [HttpGet("{adherentId}/{disciplineId}", Name = nameof(GetInscriptionByKeys))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApprendreDto>> GetInscriptionByKeys(int adherentId, int disciplineId)
        {
            var inscription = await _apprendreService.GetInscriptionByIdsAsync(adherentId, disciplineId);

            if (inscription == null)
            {
                return NotFound($"L'inscription pour l'Adhérent {adherentId} et la Discipline {disciplineId} n'a pas été trouvée.");
            }

            // Mapping vers le DTO de réponse
            return Ok(_mapper.Map<ApprendreDto>(inscription));
        }

        // POST: api/Apprendre
        // Crée une nouvelle inscription
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApprendreDto>> CreateInscription([FromBody] ApprendreCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdInscription = await _apprendreService.CreateInscriptionAsync(dto);

            // Mapping du Modèle retourné vers le DTO de réponse
            var createdInscriptionDto = _mapper.Map<ApprendreDto>(createdInscription);

            // Utilise les deux clés pour créer l'URI de la nouvelle ressource (ligne corrigée)
            return CreatedAtAction(
                nameof(GetInscriptionByKeys), 
                new { 
                    adherentId = createdInscription.AdherentId, 
                    disciplineId = createdInscription.DisciplineId 
                }, 
                createdInscriptionDto
            );
        }

        // DELETE: api/Apprendre/{adherentId}/{disciplineId}
        // Supprime une inscription par la clé composite
        [HttpDelete("{adherentId}/{disciplineId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInscription(int adherentId, int disciplineId)
        {
            var success = await _apprendreService.DeleteInscriptionAsync(adherentId, disciplineId);

            if (!success)
            {
                return NotFound($"L'inscription pour l'Adhérent {adherentId} et la Discipline {disciplineId} n'a pas été trouvée ou la suppression a échoué.");
            }

            return NoContent(); // 204 No Content pour une suppression réussie
        }
    }
}
