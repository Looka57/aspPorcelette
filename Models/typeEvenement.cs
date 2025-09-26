using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class TypeEvenement
    {
        // Propriétés de la table (Colonnes)
        public int TypeEvenementId { get; set; }
        public string Libelle { get; set; }

        // -----------------------------------------------------------------
        // Propriété de Navigation (Lien 1-à-N : Un type a plusieurs événements)
        // -----------------------------------------------------------------
        
        public ICollection<Evenement> EvenementsAssocies { get; set; } = new List<Evenement>();
    }
}