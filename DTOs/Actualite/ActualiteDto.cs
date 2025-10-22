using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Evenement;
using ASPPorcelette.API.DTOs.User; // On importe les DTOs User
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
        public DateTime DateDePublication { get; set; } // ⚠️ Correction de la casse pour la BDD
        // Remplacement de SenseiDto par UserDto (avec rôle Sensei)
        public UserDto User { get; set; } = null!; 

        // Ajoutez l'événement associé (si nécessaire sur la carte)
        public int? EvenementId { get; set; } // La clé étrangère
        public EvenementDto? EvenementAssocie { get; set; } // L'objet de navigation

        // // Optionnel : discipline liée
        // public DisciplineDto? Discipline { get; set; }
    }
}
