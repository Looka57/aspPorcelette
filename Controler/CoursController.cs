using ASPPorcelette.API.DTOs.Cours;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursController : ControllerBase
    {
        private readonly ICoursService _coursService;
        private readonly IMapper _mapper;

        public CoursController(ICoursService coursService, IMapper mapper)
        {
            _coursService = coursService;
            _mapper = mapper;
        }

        // GET: api/Cours
        /// <summary>
        /// Récupère la liste de tous les cours avec les détails Sensei, Discipline et Horaires.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoursDto>>> GetAllCours()
        {
            var cours = await _coursService.GetAllAsync();
            // Mappe la liste des modèles vers la liste des DTOs de réponse
            return Ok(_mapper.Map<IEnumerable<CoursDto>>(cours)); 
        }

        // GET: api/Cours/5
        /// <summary>
        /// Récupère un cours spécifique par ID avec tous les détails.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CoursDto>> GetCoursById(int id)
        {
            var cours = await _coursService.GetByIdAsync(id);
            if (cours == null)
            {
                return NotFound();
            }
            // Mappe le modèle vers le DTO de réponse
            return Ok(_mapper.Map<CoursDto>(cours));
        }

        // POST: api/Cours
        /// <summary>
        /// Crée un nouveau cours. Nécessite DisciplineId et SenseiId existants.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CoursDto>> CreateCours(CoursCreateDto coursDto)
        {
            // Mappe le DTO de création vers le modèle d'entité
            var coursEntity = _mapper.Map<Models.Cours>(coursDto);
            
            var createdCours = await _coursService.AddAsync(coursEntity);
            
            // Recharger le cours avec les relations Sensei et Discipline pour le renvoyer
            var coursWithDetails = await _coursService.GetByIdAsync(createdCours.CoursId);
            
            // Mappe le modèle avec détails vers le DTO de réponse
            var coursToReturn = _mapper.Map<CoursDto>(coursWithDetails);
            
            return CreatedAtAction(
                nameof(GetCoursById), 
                new { id = coursToReturn.CoursId }, 
                coursToReturn
            );
        }

        // PUT: api/Cours/5
        /// <summary>
        /// Met à jour complètement un cours existant par ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCours(int id, CoursUpdateDto coursDto)
        {
            var coursEntity = await _coursService.GetByIdAsync(id);
            if (coursEntity == null)
            {
                return NotFound();
            }

            // Mappe le DTO sur l'entité existante (met à jour les propriétés)
            _mapper.Map(coursDto, coursEntity);

            await _coursService.UpdateCours(coursEntity);
            
            return NoContent(); 
        }

        // PATCH: api/Cours/5
        /// <summary>
        /// Met à jour partiellement un cours existant par ID (JSON Patch).
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<CoursDto>> PartialUpdateCours(
            int id,
            [FromBody] JsonPatchDocument<CoursUpdateDto> patchDocument
        )
        {
            var (updatedCours, success) = await _coursService.PartialUpdateCoursAsync(id, patchDocument);
            if (!success || updatedCours == null)
            {
                return NotFound();
            }
            // Mappe l'entité mise à jour vers le DTO de réponse
            return Ok(_mapper.Map<CoursDto>(updatedCours));
        }

        // DELETE: api/Cours/5
        /// <summary>
        /// Supprime un cours par ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCours(int id)
        {
            var cours = await _coursService.DeleteCours(id);
            if (cours == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
