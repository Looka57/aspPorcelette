using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Compte
    {
        // Propriétés de la table (Colonnes)
        public int CompteId { get; set; }
        public string Nom { get; set; }
        public decimal Solde { get; set; } // Type 'decimal' pour la précision monétaire
        public decimal Epargne { get; set; } // Ajouté pour correspondre au MLD
        // Note : Les champs 'TypeCompte' et 'DateCreation' ont été retirés pour coller au MLD

        // -----------------------------------------------------------------
        // Propriétés de Navigation (Relation 1-à-N : Un compte a plusieurs transactions)
        // -----------------------------------------------------------------
        
        // La liste des transactions rattachées à ce compte
        public ICollection<Transaction> TransactionsEffectuees { get; set; } = new List<Transaction>();
    }
}