using ASPPorcelette.API.DTOs.Tarif;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Utilisation d'un namespace clair pour les contrôleurs
namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifController : ControllerBase
    {
        private readonly ITarifService _tarifService;
        private readonly IMapper _mapper;

        public TarifController(ITarifService tarifService, IMapper mapper)
        {
            _tarifService = tarifService;
            _mapper = mapper;
        }

        // --- GET ALL: api/tarif ---
        /// <summary>
        /// Récupère la liste de tous les tarifs avec leur discipline associée.
        /// </summary>
        /// <returns>Une liste de TarifDto.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TarifDto>))]
        public async Task<ActionResult<IEnumerable<TarifDto>>> GetAllTarifs()
        {
            var tarifs = await _tarifService.GetAllTarifsAsync();
            var tarifsDto = _mapper.Map<IEnumerable<TarifDto>>(tarifs);
            return Ok(tarifsDto);
        }

        // --- GET BY ID: api/tarif/{id} ---
        /// <summary>
        /// Récupère un tarif spécifique par son ID, y compris la discipline associée.
        /// </summary>
        /// <param name="id">L'ID du tarif.</param>
        /// <returns>Le TarifDto correspondant ou NotFound.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TarifDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TarifDto>> GetTarifById(int id)
        {
            var tarif = await _tarifService.GetTarifByIdAsync(id);
            if (tarif == null)
            {
                return NotFound($"Tarif avec l'ID {id} non trouvé.");
            }
            var tarifDto = _mapper.Map<TarifDto>(tarif);
            return Ok(tarifDto);
        }

        // --- POST: api/tarif ---
        /// <summary>
        /// Crée un nouveau tarif.
        /// </summary>
        /// <param name="tarifCreateDto">Les données de création du tarif.</param>
        /// <returns>Le TarifDto créé avec l'URI de localisation.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TarifDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))] // Pour Discipline non trouvée
        public async Task<ActionResult<TarifDto>> CreateTarif([FromBody] TarifCreateDto tarifCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTarif = await _tarifService.CreateTarifAsync(tarifCreateDto);
                var TarifDto = _mapper.Map<TarifDto>(createdTarif);

                // Retourne 201 Created avec l'emplacement de la nouvelle ressource
                return CreatedAtAction(nameof(GetTarifById), new { id = TarifDto.TarifId }, TarifDto);
            }
            catch (KeyNotFoundException ex)
            {
                // Capture l'exception levée dans le service si la DisciplineId n'existe pas
                return NotFound(ex.Message); 
            }
        }

        // --- PUT: api/tarif/{id} ---
        /// <summary>
        /// Met à jour intégralement un tarif existant.
        /// </summary>
        /// <param name="id">L'ID du tarif à mettre à jour.</param>
        /// <param name="tarifUpdateDto">Les données de mise à jour du tarif.</param>
        /// <returns>NoContent si succès, ou NotFound.</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult> UpdateTarif(int id, [FromBody] TarifUpdateDto tarifUpdateDto)
        {
            // NOTE: La vérification de l'ID a été supprimée car TarifUpdateDto ne contient pas de TarifId.
            // L'ID est passé via la route et utilisé directement par le service.

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var updatedTarif = await _tarifService.UpdateTarifAsync(id, tarifUpdateDto);

                if (updatedTarif == null)
                {
                    return NotFound($"Tarif avec l'ID {id} non trouvé.");
                }
                
                return NoContent(); // 204 No Content est standard pour une mise à jour réussie
            }
            catch (KeyNotFoundException ex)
            {
                 // Capture l'exception levée dans le service si la nouvelle DisciplineId n'existe pas
                return NotFound(ex.Message); 
            }
        }

        // --- PATCH: api/tarif/{id} ---
        /// <summary>
        /// Met à jour partiellement un tarif existant.
        /// </summary>
        /// <param name="id">L'ID du tarif à modifier.</param>
        /// <param name="patchDocument">Le document de patch JSON.</param>
        /// <returns>NoContent si succès, ou NotFound/BadRequest.</returns>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PartialUpdateTarif(
            int id, 
            [FromBody] JsonPatchDocument<TarifUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Le document de patch est requis.");
            }

            var (tarif, success) = await _tarifService.PartialUpdateTarifAsync(id, patchDocument);

            if (!success)
            {
                // Si l'entité n'est pas trouvée, ou si une validation de clé étrangère échoue dans le service
                return tarif == null ? NotFound($"Tarif avec l'ID {id} non trouvé.") : BadRequest("Échec de l'application du patch ou de la validation des données.");
            }

            // Validation du modèle après le patch
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return NoContent(); // 204 No Content
        }

        // --- DELETE: api/tarif/{id} ---
        /// <summary>
        /// Supprime un tarif par son ID.
        /// </summary>
        /// <param name="id">L'ID du tarif à supprimer.</param>
        /// <returns>NoContent si succès, ou NotFound.</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTarif(int id)
        {
            var isDeleted = await _tarifService.DeleteTarifAsync(id);

            if (!isDeleted)
            {
                return NotFound($"Tarif avec l'ID {id} non trouvé.");
            }

            return NoContent(); // 204 No Content est standard pour une suppression réussie
        }
    }
}
