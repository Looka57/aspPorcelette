using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Discipline
    {
        // Propriétés de la table (Colonnes)
        public int DisciplineId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens 1-à-N et N-à-N)
        // -----------------------------------------------------------------
        
        // 1. Cours (Relation 1-à-N : Une discipline a plusieurs cours)
        public ICollection<Cours> CoursAssocies { get; set; } = new List<Cours>();
        
        // 2. Sensei (Relation 1-à-N : Plusieurs sensei enseignent cette discipline principale)
        public ICollection<Sensei> SenseisEnseignants { get; set; } = new List<Sensei>();

        // 3. Événements (Relation 1-à-N : Une discipline peut avoir plusieurs événements)
        public ICollection<Evenement> EvenementsAssocies { get; set; } = new List<Evenement>();
        
        // 4. Adhérents (Relation N-à-N : Une discipline est apprise par plusieurs adhérents)
        public ICollection<Adherent> AdherentsApprenant { get; set; } = new List<Adherent>();
        
        // 5. Tarifs (Relation 1-à-N : Une discipline a plusieurs tarifs)
        public ICollection<Tarif> Tarifs { get; set; } = new List<Tarif>();
        
        // 6. Transactions (Relation 1-à-N : Une discipline peut être associée à plusieurs transactions)
        public ICollection<Transaction> TransactionsAssociees { get; set; } = new List<Transaction>();
    }
}