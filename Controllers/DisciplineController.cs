
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplineController : ControllerBase
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IMapper _mapper;
        public DisciplineController(IDisciplineService disciplineService, IMapper mapper)
        {
            _disciplineService = disciplineService;
            _mapper = mapper;
        }

        // GET: api/Discipline
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisciplineDto>>> GetDisciplines()
        {
            var disciplineModels = await _disciplineService.GetAllDisciplinesAsync();
            if (disciplineModels == null)
            {
                return NotFound();
            }

            var disciplineDtos = _mapper.Map<IEnumerable<DisciplineDto>>(disciplineModels);
            return Ok(disciplineDtos);
        }
    }
}
