using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.API.DTOs.Cours
{
    public class CoursCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Libelle { get; set; } = string.Empty;

        [Required]
        public int DisciplineId { get; set; }
        
        [Required]
        public int SenseiId { get; set; }
        
    }
}