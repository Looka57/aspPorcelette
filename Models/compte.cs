using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{

    public class Compte
    {
        [Key]
        public int CompteId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nom { get; set; } = string.Empty;

        // Le type de compte est maintenant obligatoire
        [Required]
        public TypeCompte TypeCompte { get; set; }

        // Solde initial (stocké en DB sous ce nom, précédemment 'SoldeInitial')
        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Pour garantir la précision monétaire
        public decimal Solde { get; set; } = 0.00m; 
        
        // Montant d'épargne dédié à ce compte (s'il s'agit d'un compte Épargne, sinon 0)
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Epargne { get; set; } = 0.00m; 

        // -----------------------------------------------------------------
        // Propriétés de Navigation
        // -----------------------------------------------------------------
        
        // Liste des transactions associées à ce compte
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
