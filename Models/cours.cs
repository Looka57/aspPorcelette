using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Cours
    {
        // Propriétés de la table (Colonnes)
        public int CoursId { get; set; } // Renommé pour la convention .NET (CoursId)
        public string Libelle { get; set; }

        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------
        
        // 1. Clé vers la Discipline (Relation 1,1 : Un cours est lié à une discipline)
        public int DisciplineId { get; set; }

        // 2. Clé vers le Sensei (Relation 1,1 : Un cours est enseigné par un sensei)
        public int SenseiId { get; set; }

        
        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens avec d'autres classes)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails de la Discipline associée
        public Discipline Discipline { get; set; }

        // Permet d'accéder aux détails du Sensei qui enseigne ce cours
        public Sensei Sensei { get; set; }

        // Relation 1-à-N : Un cours a plusieurs horaires (basé sur notre discussion de la règle métier)
        public ICollection<Horaire> Horaires { get; set; } = new List<Horaire>();
    }
}