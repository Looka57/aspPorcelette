using ASPPorcelette.API.DTOs.CategorieTransaction;
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
    public class CategorieTransactionController : ControllerBase
    {
        private readonly ICategorieTransactionService _categorieService;
        private readonly IMapper _mapper;

        public CategorieTransactionController(ICategorieTransactionService categorieService, IMapper mapper)
        {
            _categorieService = categorieService;
            _mapper = mapper;
        }

        // GET: api/CategorieTransaction
        /// <summary>
        /// Récupère la liste de toutes les catégories de transaction.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategorieTransactionDto>>> GetAllCategories()
        {
            var categories = await _categorieService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CategorieTransactionDto>>(categories));
        }

        // GET: api/CategorieTransaction/5
        /// <summary>
        /// Récupère une catégorie de transaction par ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategorieTransactionDto>> GetCategorieById(int id)
        {
            var categorie = await _categorieService.GetByIdAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CategorieTransactionDto>(categorie));
        }

        // POST: api/CategorieTransaction
        /// <summary>
        /// Crée une nouvelle catégorie de transaction.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategorieTransactionDto>> CreateCategorie(CategorieTransactionCreateDto createDto)
        {
            var createdCategorie = await _categorieService.CreateAsync(createDto);
            
            var categorieToReturn = _mapper.Map<CategorieTransactionDto>(createdCategorie);
            
            return CreatedAtAction(
                nameof(GetCategorieById), 
                new { id = categorieToReturn.CategorieTransactionId }, 
                categorieToReturn
            );
        }

        // PUT: api/CategorieTransaction/5
        /// <summary>
        /// Met à jour complètement une catégorie de transaction existante par ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategorie(int id, CategorieTransactionUpdateDto updateDto)
        {
            var success = await _categorieService.UpdateAsync(id, updateDto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); 
        }

        // PATCH: api/CategorieTransaction/5
        /// <summary>
        /// Met à jour partiellement une catégorie de transaction existante par ID (JSON Patch).
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<CategorieTransactionDto>> PartialUpdateCategorie(
            int id,
            [FromBody] JsonPatchDocument<CategorieTransactionUpdateDto> patchDocument
        )
        {
            var (updatedCategorie, success) = await _categorieService.PartialUpdateAsync(id, patchDocument);
            if (!success || updatedCategorie == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CategorieTransactionDto>(updatedCategorie));
        }

        // DELETE: api/CategorieTransaction/5
        /// <summary>
        /// Supprime une catégorie de transaction par ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            var success = await _categorieService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
