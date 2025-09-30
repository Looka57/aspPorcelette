// DTO de Mise à Jour (PUT/PATCH)
using System.ComponentModel.DataAnnotations;

public class TransactionUpdateDto
    {
        [Range(typeof(decimal), "-999999999.99", "999999999.99", ErrorMessage = "Le montant est invalide.")]
        public decimal? Montant { get; set; }

        [MaxLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string? Description { get; set; }
        
        public DateTimeOffset? DateTransaction { get; set; }
        
        // On permet de changer les clés étrangères, ce qui revient à reclasser la transaction.
        public int? CompteId { get; set; }
        public int? CategorieTransactionId { get; set; }
        public int? DisciplineId { get; set; }
    }