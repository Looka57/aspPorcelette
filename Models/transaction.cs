using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPPorcelette.API.Models.Identity;

namespace ASPPorcelette.API.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public DateTime DateTransaction { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Montant { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // -----------------------------------------------------------------
        // Clés étrangères
        // -----------------------------------------------------------------
        [Required]
        public int CompteId { get; set; }

        [Required]
        public int CategorieTransactionId { get; set; }

        [Required]
        public int DisciplineId { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        // -----------------------------------------------------------------
        // Propriétés de navigation
        // -----------------------------------------------------------------
        [ForeignKey(nameof(CompteId))]
        public Compte Compte { get; set; } = null!;

        [ForeignKey(nameof(CategorieTransactionId))]
        public CategorieTransaction Categorie { get; set; } = null!;

        [ForeignKey(nameof(DisciplineId))]
        public Discipline Discipline { get; set; } = null!;
    }
}
