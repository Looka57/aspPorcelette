using System;

namespace ASPPorcelette.API.Models
{
    public class Horaire
    {
        // Propriétés de la table (Colonnes)
        public int HoraireId { get; set; }
        public string Jour { get; set; } // Ex: "Lundi", "Mercredi"
        public TimeSpan HeureDebut { get; set; } // Utilisation de TimeSpan pour l'heure seule
        public TimeSpan HeureFin { get; set; } // Utilisation de TimeSpan pour l'heure seule

        // -----------------------------------------------------------------
        // Clé Étrangère (Foreign Key)
        // -----------------------------------------------------------------
        
        // Clé vers le Cours (L'horaire appartient à ce Cours)
        public int CoursId { get; set; }

        
        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien vers la classe parente)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails du Cours associé
        public Cours Cours { get; set; }
    }
}