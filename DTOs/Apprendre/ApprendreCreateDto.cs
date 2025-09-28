using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Apprendre
{
    public class ApprendreCreateDto
    {
        // Clés étrangères requises
        [Required]
        public int AdherentId { get; set; }
        
        [Required]
        public int DisciplineId { get; set; }
    }
}
