using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; 
using System.IO; 
using System;
using System.Security.Claims; // 🛑 Ajouté pour les claims d'utilisateur
using Microsoft.AspNetCore.Authorization; // 🛑 Ajouté pour l'attribut [Authorize]

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualiteController : ControllerBase
    {
        private readonly IActualiteService _actualiteService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env; 

        // -----------------------------------------------------------------
        // CONSTRUCTEUR
        // -----------------------------------------------------------------
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

        // -----------------------------------------------------------------
        // GET: Toutes les actualités
        // -----------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActualiteDto>>> GetAllActualites()
        {
            var actualites = await _actualiteService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ActualiteDto>>(actualites));
        }

        // -----------------------------------------------------------------
        // GET: Une actualité spécifique par ID
        // -----------------------------------------------------------------
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

        // -----------------------------------------------------------------
        // POST: Créer une nouvelle actualité
        // -----------------------------------------------------------------
        [HttpPost]
        [Authorize] // 🛑 Assure qu'un token est présent et valide
        [Consumes("multipart/form-data")] 
        public async Task<ActionResult<ActualiteDto>> CreateActualite([FromForm] ActualiteCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // -----------------------------------------------------------------
            // 🛑 CORRECTION CLÉ : EXTRACTION ET ASSIGNATION DU USER ID (Sensei)
            // -----------------------------------------------------------------
            // 1. Récupère l'ID de l'utilisateur à partir du token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                // Si l'ID est manquant (token mal formé ou autre), renvoyer une erreur d'autorisation
                return Unauthorized();
            }

            // 2. Assigne l'ID de l'utilisateur connecté au DTO. 
            //    Ceci écrase toute valeur potentielle dans le corps de la requête.
            createDto.UserId = userId; // 🛑 Vous devez ajouter public string UserId { get; set; } à ActualiteCreateDto
            // -----------------------------------------------------------------

            string imageUrl = null;

            // Gestion du fichier image
            if (createDto.ImageFile != null)
            {
                var uploadFolder = Path.Combine(_env.WebRootPath, "images", "actualites");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(createDto.ImageFile.FileName);
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await createDto.ImageFile.CopyToAsync(fileStream);
                }

                imageUrl = $"/images/actualites/{uniqueFileName}";
            }

            createDto.ImageUrl = imageUrl;
            
            // Le service doit maintenant prendre en charge le DTO qui contient le UserId correct.
            var createdActualite = await _actualiteService.CreateAsync(createDto);

            return CreatedAtAction(
                nameof(GetActualiteById),
                new { id = createdActualite.ActualiteId },
                _mapper.Map<ActualiteDto>(createdActualite)
            );
        }

        // -----------------------------------------------------------------
        // PUT: Mise à jour complète d'une actualité existante
        // -----------------------------------------------------------------
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateActualite(int id, [FromForm] ActualiteUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // NOTE: Pour la mise à jour, vous devriez aussi vérifier que l'utilisateur connecté est bien l'auteur.
            
            var success = await _actualiteService.UpdateAsync(id, updateDto, _env.WebRootPath);

            if (!success)
            {
                return NotFound();
            }

            var updatedActualite = await _actualiteService.GetByIdAsync(id); 
            return Ok(_mapper.Map<ActualiteDto>(updatedActualite));
        }

        // -----------------------------------------------------------------
        // PATCH: Mise à jour partielle d'une actualité (JSON Patch)
        // -----------------------------------------------------------------
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

        // -----------------------------------------------------------------
        // DELETE: Supprimer une actualité par ID
        // -----------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActualite(int id)
        {
            var success = await _actualiteService.DeleteAsync(id, _env.WebRootPath);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}