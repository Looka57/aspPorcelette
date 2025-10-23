using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // 🎯 NOUVEAU
using System.IO; 
using System;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualiteController : ControllerBase
    {
        private readonly IActualiteService _actualiteService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env; // 🎯 NOUVEAU : Pour l'environnement hôte
public ActualiteController(
            IActualiteService actualiteService, 
            IMapper mapper,
            IWebHostEnvironment env
             ) 
        {
            _actualiteService = actualiteService;
            _mapper = mapper;
            _env = env;
            
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
        [Consumes("multipart/form-data")] // 🎯 ESSENTIEL : Résout le 415
        public async Task<ActionResult<ActualiteDto>> CreateActualite([FromForm] ActualiteCreateDto createDto) 
        // 🎯 ESSENTIEL : Résout le 415
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            string imageUrl = null;

            // -----------------------------------------------------
            // 🎯 LOGIQUE DE SAUVEGARDE DU FICHIER (Méthode Profil simplifiée)
            // -----------------------------------------------------
            if (createDto.ImageFile != null)
            {
                // 1. Définir le chemin du dossier cible (ex: dans wwwroot)
                var uploadFolder = Path.Combine(_env.WebRootPath, "images", "actualites");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                
                // 2. Créer un nom de fichier unique
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(createDto.ImageFile.FileName);
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                // 3. Sauvegarder le fichier sur le disque
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createDto.ImageFile.CopyToAsync(fileStream);
                }

                // 4. Définir l'URL relative à stocker en base de données
                imageUrl = $"/images/actualites/{uniqueFileName}";
            }
            // -----------------------------------------------------
            
            // Assigner l'URL générée au DTO AVANT de l'envoyer au service
            createDto.ImageUrl = imageUrl; 

            var createdActualite = await _actualiteService.CreateAsync(createDto);

            // ... (suite inchangée) ...
            
            return CreatedAtAction(
                nameof(GetActualiteById), 
                new { id = createdActualite.ActualiteId }, // Utilisez createdActualite.ActualiteId
                _mapper.Map<ActualiteDto>(createdActualite)
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
