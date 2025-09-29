// Fichier : Controllers/TypeEvenementController.cs
using ASPPorcelette.API.DTOs.TypeEvenement;
using ASPPorcelette.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeEvenementController : ControllerBase
    {
        private readonly ITypeEvenementService _service;

        public TypeEvenementController(ITypeEvenementService service)
        {
            _service = service;
        }

        /// <summary>
        /// Récupère la liste de tous les types d'événements.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var types = await _service.GetAllTypesAsync();
            return Ok(types);
        }

        /// <summary>
        /// Récupère un type d'événement spécifique par son ID.
        /// </summary>
        /// <param name="id">ID du type d'événement.</param>
        [HttpGet("{id}")]
   
        public async Task<IActionResult> GetById(int id)
        {
            var typeEvenement = await _service.GetTypeByIdAsync(id);

            if (typeEvenement == null)
            {
                return NotFound();
            }

            return Ok(typeEvenement);
        }

        /// <summary>
        /// Crée un nouveau type d'événement.
        /// </summary>
        /// <param name="createDto">Les données du type d'événement à créer.</param>
        [HttpPost]
   
        public async Task<IActionResult> Create([FromBody] TypeEvenementCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdType = await _service.CreateTypeAsync(createDto);

            // Retourne 201 Created avec l'emplacement de la nouvelle ressource
            return CreatedAtAction(nameof(GetById), new { id = createdType.TypeEvenementId }, createdType);
        }

  /// <summary>
        /// Met à jour complètement un type d'événement existant.
        /// </summary>
        /// <param name="id">ID du type d'événement à mettre à jour.</param>
        /// <param name="updateDto">Les nouvelles données du type d'événement.</param>
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] TypeEvenementUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // CORRECTION : Utiliser 'updateDto' (la variable) au lieu de 'TypeEvenementUpdateDto' (le type)
            var success = await _service.UpdateTypeAsync(id, updateDto); 

            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // Code 204 si la mise à jour est réussie
        }

        // --- NOUVELLE ACTION : SUPPRESSION (DELETE) ---
        /// <summary>
        /// Supprime un type d'événement par son ID.
        /// </summary>
        /// <param name="id">ID du type d'événement à supprimer.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)] // No Content
        [ProducesResponseType(404)] // Not Found
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteTypeAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent(); // Code 204 si la suppression est réussie
        }

    }
}

