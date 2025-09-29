using System;
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.Evenement
{
    public class EvenementCreateDto
    {
        [Required(ErrorMessage = "Le titre est requis.")]
        [StringLength(100)]
        public string Titre { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de début est requise.")]
        public DateTime DateDebut { get; set; }

        public DateTime? DateFin { get; set; }

        [Required(ErrorMessage = "L'URL de l'image est requise.")]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type d'événement est requis.")]
        public int TypeEvenementId { get; set; }

        public int? DisciplineId { get; set; }

    }
}
