using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class CategorieTransaction
    {
        // Propriétés de la table (Colonnes)
        public int CategorieTransactionId { get; set; }
        public string Nom { get; set; }
        public string TypeCategorie { get; set; } // Par exemple: "Recette" ou "Dépense"

        // -----------------------------------------------------------------
        // Propriétés de Navigation (Relation 1-à-N : Une catégorie a plusieurs transactions)
        // -----------------------------------------------------------------
        
        // Initialisation nécessaire pour éviter les NullReferenceException lors de l'ajout
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}