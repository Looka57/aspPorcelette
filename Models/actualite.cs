using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Actualite
    {
        [Key]
        public int ActualiteId { get; set; }

        [Required(ErrorMessage = "Le titre est requis.")]
        [MaxLength(150)]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le contenu est requis.")]
        public string Contenu { get; set; } = string.Empty;

        // Note : Nom de colonne simplifié à DatePublication, car DateDePublication est redondant.
        [Required]
        public DateTime DateDePublication { get; set; } = DateTime.UtcNow; 
        
        [MaxLength(500)]
        public string? ImageUrl { get; set; }


        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------

        // L'auteur de l'actualité (requis)
        [ForeignKey(nameof(SenseiAuteur))]
        public int SenseiId { get; set; }

        // L'actualité peut concerner un événement (optionnel)
        [ForeignKey(nameof(EvenementAssocie))]
        public int? EvenementId { get; set; }
        
        

        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens)
        // -----------------------------------------------------------------

        // Permet d'accéder aux détails du Sensei qui a écrit l'actualité
        public Sensei SenseiAuteur { get; set; } = null!; // SenseiId est requis, donc SenseiAuteur l'est aussi.

        // Permet d'accéder aux détails de l'événement associé (peut être null)
        public Evenement? EvenementAssocie { get; set; }
        
 
    }
}
