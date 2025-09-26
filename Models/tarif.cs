namespace ASPPorcelette.API.Models
{
    public class Tarif
    {
        // Propriétés de la table (Colonnes)
        public int TarifId { get; set; }
        public string Libelle { get; set; }
        public decimal Prix { get; set; } // Utilisation de decimal pour le montant
        public string Description { get; set; }

        // -----------------------------------------------------------------
        // Clé Étrangère (Foreign Key)
        // -----------------------------------------------------------------
        
        // Clé vers la Discipline (Relation 1,1 : Un tarif est lié à une seule discipline)
        public int DisciplineId { get; set; }

        
        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien vers la classe parente)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails de la Discipline concernée
        public Discipline DisciplineConcernee { get; set; }
    }
}