using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.DTOs.Discipline;

namespace ASPPorcelette.API.DTOs.Sensei
{
 public class SenseiMinimalDto
    {
        public string Id { get; set; } = string.Empty; 
        public string Prenom { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string? Grade { get; set; }
        public string? PhotoUrl { get; set; } // Si vous affichez une photo

    }
}