using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Sensei;
using System;
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Actualite
{
    // DTO de Réponse (Sortie)
    public class ActualiteDto
    {
        public int ActualiteId { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Contenu { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime DatePublication { get; set; }

        // Relations Détaillées pour la sortie
        public SenseiDto Sensei { get; set; } = null!;
        public DisciplineDto? Discipline { get; set; }
    }

   

}
