using ASPPorcelette.API.DTOs.Actualite;
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
    public class ActualiteController : ControllerBase
    {
        private readonly IActualiteService _actualiteService;
        private readonly IMapper _mapper;

        public ActualiteController(IActualiteService actualiteService, IMapper mapper)
        {
            _actualiteService = actualiteService;
            _mapper = mapper;
        }

        // GET: api/Actualite
        /// <summary>
        /// Récupère toutes les actualités avec les détails du Sensei, de l'Événement et de la Discipline associés.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActualiteDto>>> GetAllActualites()
        {
            var actualites = await _actualiteService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ActualiteDto>>(actualites));
        }

        // GET: api/Actualite/5
        /// <summary>
        /// Récupère une actualité spécifique par ID avec tous les détails.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ActualiteDto>> GetActualiteById(int id)
        {
            var actualite = await _actualiteService.GetByIdAsync(id);
            if (actualite == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ActualiteDto>(actualite));
        }

        // POST: api/Actualite
        /// <summary>
        /// Crée une nouvelle actualité.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ActualiteDto>> CreateActualite(ActualiteCreateDto createDto)
        {
            // Vérification de la validité de l'objet de création (SenseiId, etc.)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdActualite = await _actualiteService.CreateAsync(createDto);

            // Recharger l'entité avec les relations pour le retour au client
            var actualiteWithDetails = await _actualiteService.GetByIdAsync(createdActualite.ActualiteId);

            var actualiteToReturn = _mapper.Map<ActualiteDto>(actualiteWithDetails);
            
            return CreatedAtAction(
                nameof(GetActualiteById), 
                new { id = actualiteToReturn.ActualiteId }, 
                actualiteToReturn
            );
        }

        // PUT: api/Actualite/5
        /// <summary>
        /// Met à jour complètement une actualité existante.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActualite(int id, ActualiteUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var success = await _actualiteService.UpdateAsync(id, updateDto);
            
            if (!success)
            {
                return NotFound();
            }

            // Retourne 204 No Content si la mise à jour a réussi
            return NoContent();
        }

        // PATCH: api/Actualite/5
        /// <summary>
        /// Met à jour partiellement une actualité (JSON Patch).
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<ActualiteDto>> PartialUpdateActualite(
            int id,
            [FromBody] JsonPatchDocument<ActualiteUpdateDto> patchDocument
        )
        {
            var (updatedActualite, success) = await _actualiteService.PartialUpdateAsync(id, patchDocument);

            if (!success || updatedActualite == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<ActualiteDto>(updatedActualite));
        }

        // DELETE: api/Actualite/5
        /// <summary>
        /// Supprime une actualité par ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActualite(int id)
        {
            var success = await _actualiteService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
