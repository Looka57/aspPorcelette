using ASPPorcelette.API.DTOs.Adherent;
using ASPPorcelette.API.DTOs.Discipline; // Assurez-vous d'avoir un DTO pour Discipline
using System;

namespace ASPPorcelette.API.DTOs.Apprendre
{
    public class ApprendreDto
    {
        public int ApprendreId { get; set; }

        // Relations (Utilise les DTOs pour éviter la boucle)
        public AdherentDto Adherent { get; set; } 
        public DTOs.Discipline.DisciplineDto Discipline { get; set; } // Utilisez le namespace correct
    }
}
