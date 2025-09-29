// DTO de Création (Entrée POST)
using System.ComponentModel.DataAnnotations;

public class ActualiteCreateDto
    {
        [Required(ErrorMessage = "Le titre est requis.")]
        [StringLength(150)]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le contenu est requis.")]
        public string Contenu { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "L'ID du Sensei auteur est requis.")]
        public int SenseiId { get; set; }

        public int? DisciplineId { get; set; }
    }