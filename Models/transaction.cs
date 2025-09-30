using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPPorcelette.API.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public DateTime DateTransaction { get; set; } = DateTime.UtcNow; 

        // Le montant peut être positif (revenu) ou négatif (dépense)
        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Précision monétaire
        public decimal Montant { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------
        
        // 1. Clé étrangère vers le Compte (Transaction N-1 Compte)
        [Required]
        public int CompteId { get; set; }

        // 2. Clé étrangère vers la Catégorie (Transaction N-1 CatégorieTransaction)
        [Required]
        public int CategorieTransactionId { get; set; }
        
        // 3. NOUVEAU: Clé étrangère vers la Discipline (Transaction N-1 Discipline)
        // Peut être requis si toutes les transactions sont liées à une discipline (cotisations, etc.)
        [Required] 
        public int DisciplineId { get; set; }


        // -----------------------------------------------------------------
        // Propriétés de Navigation
        // -----------------------------------------------------------------

        // Le Compte concerné (Relation N-1)
        [ForeignKey(nameof(CompteId))]
        public Compte Compte { get; set; } = null!;
        
        // La Catégorie de la transaction (Relation N-1)
        [ForeignKey(nameof(CategorieTransactionId))]
        public CategorieTransaction Categorie { get; set; } = null!;

        // NOUVEAU: La Discipline concernée (Relation N-1)
        [ForeignKey(nameof(DisciplineId))]
        public Discipline Discipline { get; set; } = null!;
    }
}
