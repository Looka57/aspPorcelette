using ASPPorcelette.API.DTOs.Compte;
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
    public class CompteController : ControllerBase
    {
        private readonly ICompteService _compteService;
        private readonly IMapper _mapper;

        public CompteController(ICompteService compteService, IMapper mapper)
        {
            _compteService = compteService;
            _mapper = mapper;
        }

        // GET: api/Compte
        /// <summary>
        /// Récupère la liste de tous les comptes avec leur solde actuel calculé.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompteDto>>> GetAllComptes()
        {
            // Appelle la méthode qui gère le calcul du solde
            var comptesDto = await _compteService.GetAllWithBalanceAsync();
            return Ok(comptesDto);
        }

        // GET: api/Compte/5
        /// <summary>
        /// Récupère un compte par ID avec son solde actuel calculé.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CompteDto>> GetCompteById(int id)
        {
            // Appelle la méthode qui gère le calcul du solde
            var compteDto = await _compteService.GetByIdWithBalanceAsync(id);
            if (compteDto == null)
            {
                return NotFound();
            }
            return Ok(compteDto);
        }

        // POST: api/Compte
        /// <summary>
        /// Crée un nouveau compte.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CompteDto>> CreateCompte(CompteCreateDto createDto)
        {
            var createdCompte = await _compteService.CreateAsync(createDto);
            
            // Pour retourner l'objet avec le SoldeActuel calculé, on le récupère après la création
            var compteToReturn = await _compteService.GetByIdWithBalanceAsync(createdCompte.CompteId);
            
            return CreatedAtAction(
                nameof(GetCompteById), 
                new { id = compteToReturn?.CompteId }, 
                compteToReturn
            );
        }

        // PUT: api/Compte/5
        /// <summary>
        /// Met à jour complètement un compte existant par ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompte(int id, CompteUpdateDto updateDto)
        {
            var success = await _compteService.UpdateAsync(id, updateDto);
            if (!success)
            {
                return NotFound();
            }
            return NoContent(); 
        }

        // PATCH: api/Compte/5
        /// <summary>
        /// Met à jour partiellement un compte existant par ID (JSON Patch).
        /// </summary>
        [HttpPatch("{id}")]
        public async Task<ActionResult<CompteDto>> PartialUpdateCompte(
            int id,
            [FromBody] JsonPatchDocument<CompteUpdateDto> patchDocument
        )
        {
            var (updatedCompte, success) = await _compteService.PartialUpdateAsync(id, patchDocument);
            if (!success || updatedCompte == null)
            {
                return NotFound();
            }
            
            // Calculer le solde pour l'objet retourné
            var compteToReturn = await _compteService.GetByIdWithBalanceAsync(updatedCompte.CompteId);

            return Ok(compteToReturn);
        }

        // DELETE: api/Compte/5
        /// <summary>
        /// Supprime un compte par ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompte(int id)
        {
            var success = await _compteService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
