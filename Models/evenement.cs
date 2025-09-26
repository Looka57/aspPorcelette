using System;
using System.Collections.Generic; // Nécessaire pour la collection d'Actualités

namespace ASPPorcelette.API.Models
{
    public class Evenement
    {
        // Propriétés de la table (Colonnes)
        public int EvenementId { get; set; }
        public string Titre { get; set; }
        public string Description { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string ImageUrl { get; set; }

        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------
        
        // 1. Clé vers le Type d'Événement (Relation 1,1)
        public int TypeEvenementId { get; set; }

        // 2. Clé vers la Discipline (Relation 1,1)
        public int DisciplineId { get; set; }

        
        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens avec d'autres classes)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails du type d'événement (Ex: "Compétition", "Stage")
        public TypeEvenement TypeEvenement { get; set; }

        // Permet d'accéder aux détails de la Discipline concernée
        public Discipline DisciplineAssociee { get; set; }

        // Propriété de navigation inverse : Liste des actualités qui informent sur cet événement (Relation 1-à-N)
        public ICollection<Actualite> ActualitesLiées { get; set; } = new List<Actualite>();
    }
}