// DTO de Mise à Jour (PUT/PATCH)
using System.ComponentModel.DataAnnotations;

public class TransactionUpdateDto
    {
        public decimal? Montant { get; set; }

        [MaxLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string? Description { get; set; }
        
    public DateTime? DateTransaction { get; set; }  // <-- ici
        
        // On permet de changer les clés étrangères, ce qui revient à reclasser la transaction.
        public int? CompteId { get; set; }
        public int? CategorieTransactionId { get; set; }
        public int? DisciplineId { get; set; }
    }