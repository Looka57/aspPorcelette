using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ASPPorcelette.API.Models.Enums;

namespace ASPPorcelette.API.Models
{


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
