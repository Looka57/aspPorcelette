using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    // Énumération pour définir le type de transaction
    public enum TypeFlux
    {
        Recette, // Entrée d'argent (Revenus)
        Depense  // Sortie d'argent (Dépenses)
    }

    public class CategorieTransaction
    {
        [Key]
        public int CategorieTransactionId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nom { get; set; } 

        // Type de flux financier (Retour à l'enum)
        [Required]
        public TypeFlux TypeFlux { get; set; }

        // -----------------------------------------------------------------
        // Propriétés de Navigation
        // -----------------------------------------------------------------
        
        // Liste des transactions associées à cette catégorie
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
