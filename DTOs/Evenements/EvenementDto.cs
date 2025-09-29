using ASPPorcelette.API.DTOs.TypeEvenement;
using System;

namespace ASPPorcelette.API.DTOs.Evenement
{
    public class EvenementDto
    {
        public int EvenementId { get; set; }
        public string Titre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string ImageUrl { get; set; }
        public int? DisciplineId { get; set; }
        
        // DTO imbriqué pour afficher les informations du type d'événement
        public TypeEvenementDto TypeEvenement { get; set; } = null!;
    }
}
