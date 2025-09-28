
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPPorcelette.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SenseiController : ControllerBase
    {
        private readonly ISenseiService _senseiService;
        private readonly IMapper _mapper;

        public SenseiController(ISenseiService senseiService, IMapper mapper)
        {
            _senseiService = senseiService;
            _mapper = mapper;
        }

       // GET: api/Sensei
        // Le type de retour utilise maintenant le DTO
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SenseiDto>>> GetSenseis()
        {
            // 1. Récupère la liste des modèles complets (incluant la DisciplinePrincipale)
            var senseiModels = await _senseiService.GetAllSenseisAsync();

            if (senseiModels == null)
            {
                return NotFound();
            }

            // 2. Mappe la liste des modèles Sensei vers une liste de SenseiDto
            var senseiDtos = _mapper.Map<IEnumerable<SenseiDto>>(senseiModels);

            // 3. Retourne la liste des DTOs
            return Ok(senseiDtos);
        }


        // GET: api/Sensei/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensei>> GetSenseiById(int id)
        {
            var sensei = await _senseiService.GetSenseiByIdAsync(id);
            if (sensei == null)
            {
                return NotFound();
            }
            return Ok(sensei);
        }

        // POST: api/Sensei
        [HttpPost]
        public async Task<ActionResult<SenseiCreateDto>> CreateSensei(Sensei sensei)
        {
            var createdSensei = await _senseiService.CreateSenseiAsync(sensei);
            return CreatedAtAction(nameof(GetSenseiById), new { id = createdSensei.SenseiId }, createdSensei);
        }

        // PUT: api/Sensei/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSensei(int id, Sensei sensei)
        {
            if (id != sensei.SenseiId)
            {
                return BadRequest();
            }

            var updatedSensei = await _senseiService.UpdateSenseiAsync(sensei);
            return Ok(updatedSensei);
        }

        // PATCH: api/Sensei/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateSensei(
            int id, 
            [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<SenseiUpdateDto> patchDoc
        )
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var (updatedSensei, success) = await _senseiService.PartialUpdateSenseiAsync(id, patchDoc);
            if (!success)
            {
                return NotFound();
            }

            return Ok(updatedSensei);
        }

        // DELETE: api/Sensei/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensei(int id)
        {
            var deletedSensei = await _senseiService.DeleteSenseiAsync(id);
            if (deletedSensei == null)
            {
                return NotFound();
            }
            return Ok(deletedSensei);
        }
    }
}