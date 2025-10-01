// using ASPPorcelette.API.DTOs;
// using ASPPorcelette.API.DTOs.Adherent;
// using ASPPorcelette.API.Models;
// using ASPPorcelette.API.Repository.Interfaces;
// using AutoMapper;
// using Microsoft.AspNetCore.Mvc;

// namespace ASPPorcelette.API.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]

//     public class AdherentController : ControllerBase
//     {
//         // Implémentation du contrôleur Adherent
//         private readonly IAdherentService _adherentService;
//         private readonly IMapper _mapper;

//         public AdherentController(IAdherentService adherentService, IMapper mapper)
//         {
//             _adherentService = adherentService;
//             _mapper = mapper;
//         }

//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<AdherentDto>>> GetAllAdherents()
//         {
//             var adherents = await _adherentService.GetAllAsync();
//             return Ok(adherents);
//         }
//         [HttpGet("{id}")]
//         public async Task<ActionResult<Adherent>> GetAdherentById(int id)
//         {
//             var adherent = await _adherentService.GetByIdAsync(id);
//             if (adherent == null)
//             {
//                 return NotFound();
//             }
//             return Ok(adherent);
//         }
// // POST: api/Adherent
//         // Prend un DTO de création en entrée
//         [HttpPost]
//         public async Task<ActionResult<AdherentDto>> CreateAdherent(AdherentCreateDto adherentDto)
//         {
//             // Mappe le DTO vers le modèle d'entité
//             var adherentEntity = _mapper.Map<Models.Adherent>(adherentDto);
            
//             var createdAdherent = await _adherentService.AddAsync(adherentEntity);
            
//             // Renvoie le DTO créé et l'URL du nouvel Adhérent
//             var adherentToReturn = _mapper.Map<AdherentDto>(createdAdherent);
//             return CreatedAtAction(
//                 nameof(GetAdherentById), 
//                 new { id = adherentToReturn.AdherentId }, 
//                 adherentToReturn
//             );
//         }


// // PUT: api/Adherent/5
//         // Prend un DTO de mise à jour en entrée
//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateAdherent(int id, AdherentUpdateDto adherentDto)
//         {
//             // 1. Vérifier si l'adhérent existe
//             var adherentEntity = await _adherentService.GetByIdAsync(id);
//             if (adherentEntity == null)
//             {
//                 return NotFound();
//             }

//             // 2. Mappe le DTO (adherentDto) sur l'entité existante (adherentEntity)
//             _mapper.Map(adherentDto, adherentEntity);

//             // 3. Sauvegarder l'entité mise à jour
//             var updatedAdherent = await _adherentService.UpdateAdherent(adherentEntity);
            
//             // Réponse standard pour une mise à jour réussie
//             return NoContent(); 
//         }




//         // -----------------------------------------------------------------
//         // NOUVEAU : Méthode PATCH pour la mise à jour partielle
//         // ----------------------------------------------------------------- // PATCH: api/Adherent/5
//         // Gère la mise à jour partielle
//         [HttpPatch("{id}")]
//         public async Task<ActionResult<AdherentDto>> PartialUpdateAdherent(
//             int id,
//             Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<AdherentUpdateDto> patchDocument
//         )
//         {
//             var (updatedAdherent, success) = await _adherentService.PartialUpdateAdherentAsync(id, patchDocument);
//             if (!success || updatedAdherent == null)
//             {
//                 return NotFound();
//             }
//             return Ok(_mapper.Map<AdherentDto>(updatedAdherent));
//         }


//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteAdherent(int id)
//         {
//             var adherent = await _adherentService.GetByIdAsync(id);
//             if (adherent == null)
//             {
//                 return NotFound();
//             }

//             await _adherentService.DeleteAdherent(id);
//             return NoContent();
//         }
        

//     }
// }