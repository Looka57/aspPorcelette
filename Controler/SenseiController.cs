using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASPPorcelette.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SenseiController : ControllerBase
    {
        private readonly ISenseiService _senseiService;

        public SenseiController(ISenseiService senseiService)
        {
            _senseiService = senseiService;
        }

        // GET: api/Sensei
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensei>>> GetAllSenseis()
        {
            var senseis = await _senseiService.GetAllSenseisAsync();
            return Ok(senseis);
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
        public async Task<ActionResult<Sensei>> CreateSensei(Sensei sensei)
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