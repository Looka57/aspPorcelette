using System.ComponentModel.DataAnnotations;
using System;

namespace ASPPorcelette.API.DTOs.Cours
{
    public class CoursUpdateDto
    {
        // Pas de [Required] ici pour faciliter le PATCH
        public string? Libelle { get; set; }
        public int? DisciplineId { get; set; }
           public string? UserId { get; set; } 

    }
}