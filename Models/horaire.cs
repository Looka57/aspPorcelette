using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Horaire
    {
        // Propriétés de la table (Colonnes)
        [Key] public int HoraireId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Jour { get; set; } 

        [Required]
        public TimeSpan HeureDebut { get; set; } // Utilisation de TimeSpan pour l'heure seule

        [Required]
        public TimeSpan HeureFin { get; set; } // Utilisation de TimeSpan pour l'heure seule

        // -----------------------------------------------------------------
        // Clé Étrangère (Foreign Key)
        // -----------------------------------------------------------------

        // Clé vers le Cours (L'horaire appartient à ce Cours)
        [Required]
        public int CoursId { get; set; }


        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien vers la classe parente)
        // -----------------------------------------------------------------

        // Permet d'accéder aux détails du Cours associé
        [ForeignKey("CoursId")]
        public Cours Cours { get; set; }
    }
}