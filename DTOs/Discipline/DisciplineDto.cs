using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Cours;

namespace ASPPorcelette.API.DTOs.Discipline
{
    public class DisciplineDto
    {
        public int DisciplineId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }

    public ICollection<CoursDto> Cours { get; set; } = new List<CoursDto>();

    }
}