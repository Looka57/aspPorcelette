using ASPPorcelette.API.DTOs.Evenement;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvenementController : ControllerBase
    {
        private readonly IEvenementService _evenementService;
        private readonly IMapper _mapper;

        public EvenementController(IEvenementService evenementService, IMapper mapper)
        {
            _evenementService = evenementService;
            _mapper = mapper;
        }

        // GET: api/Evenement
        /// <summary>
        /// Récupère la liste de tous les événements avec les détails des relations (TypeEvenement, Discipline).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EvenementDto>>> GetAllEvenements()
        {
            var evenements = await _evenementService.GetAllEvenementsAsync();
            // Mappe la liste des modèles vers la liste des DTOs de réponse
            return Ok(_mapper.Map<IEnumerable<EvenementDto>>(evenements)); 
        }

        // GET: api/Evenement/5
        /// <summary>
        /// Récupère un événement spécifique par ID avec tous les détails.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EvenementDto>> GetEvenementById(int id)
        {
            var evenement = await _evenementService.GetEvenementByIdAsync(id);
            if (evenement == null)
            {
                return NotFound();
            }
            // Mappe le modèle vers le DTO de réponse
            return Ok(_mapper.Map<EvenementDto>(evenement));
        }

        // POST: api/Evenement
        /// <summary>
        /// Crée un nouvel événement. Nécessite TypeEvenementId et DisciplineId existants.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EvenementDto>> CreateEvenement([FromBody] EvenementCreateDto evenementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mappe le DTO de création vers le modèle d'entité
            var createdEntity = await _evenementService.CreateEvenementAsync(evenementDto);
            
            // Recharger l'événement avec les relations Sensei et Discipline pour le renvoyer
            // Ceci est nécessaire car la méthode CreateEvenementAsync ne retourne pas les relations chargées
            var evenementWithDetails = await _evenementService.GetEvenementByIdAsync(createdEntity.EvenementId);
            
            if (evenementWithDetails == null)
            {
                // Ceci ne devrait pas arriver si l'ajout a réussi
                return StatusCode(StatusCodes.Status500InternalServerError, "Erreur lors de la récupération de l'événement créé.");
            }
            
            // Mappe le modèle avec détails vers le DTO de réponse
            var evenementToReturn = _mapper.Map<EvenementDto>(evenementWithDetails);
            
            return CreatedAtAction(
                nameof(GetEvenementById), 
                new { id = evenementToReturn.EvenementId }, 
                evenementToReturn
            );
        }

        // PUT: api/Evenement/5
        /// <summary>
        /// Met à jour complètement un événement existant par ID.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEvenement(int id, [FromBody] EvenementUpdateDto evenementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _evenementService.UpdateEvenementAsync(id, evenementDto);
            
            if (!success)
            {
                return NotFound();
            }
            
            return NoContent(); 
        }

        // PATCH: api/Evenement/5
        /// <summary>
        /// Met à jour partiellement un événement existant par ID (JSON Patch).
        /// </summary>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EvenementDto>> PartialUpdateEvenement(
            int id,
            [FromBody] JsonPatchDocument<EvenementUpdateDto> patchDocument
        )
        {
            var (updatedEvenement, success) = await _evenementService.PartialUpdateEvenementAsync(id, patchDocument);
            
            if (!success || updatedEvenement == null)
            {
                return NotFound();
            }
            
            // Mappe l'entité mise à jour vers le DTO de réponse
            return Ok(_mapper.Map<EvenementDto>(updatedEvenement));
        }

        // DELETE: api/Evenement/5
        /// <summary>
        /// Supprime un événement par ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvenement(int id)
        {
            var success = await _evenementService.DeleteEvenementAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
