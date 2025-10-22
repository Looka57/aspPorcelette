
// DTO de Mise à Jour (Entrée PUT/PATCH)
using System.ComponentModel.DataAnnotations;

public class ActualiteUpdateDto
    {
        [Required(ErrorMessage = "Le titre est requis.")]
        [StringLength(150)]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le contenu est requis.")]
        public string Contenu { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        
        // On permet de changer l'auteur et la discipline lors de la mise à jour
       [Required(ErrorMessage = "L'ID de l'utilisateur (auteur) est requis.")]
public string UserId { get; set; } = string.Empty;


        public int? DisciplineId { get; set; }
    }