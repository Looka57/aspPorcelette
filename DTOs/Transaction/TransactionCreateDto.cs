// DTO de Création (POST)
// Nécessite le montant, les dates et toutes les clés étrangères.
using System.ComponentModel.DataAnnotations;

public class TransactionCreateDto
    {
        [Required(ErrorMessage = "Le montant est requis.")]
        [Range(typeof(decimal), "-999999999.99", "999999999.99", ErrorMessage = "Le montant est invalide.")]
        public decimal Montant { get; set; }

        [MaxLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string? Description { get; set; }
        
        // DateTransaction est optionnel ici; s'il est null, le modèle utilisera UtcNow par défaut.
        public DateTimeOffset? DateTransaction { get; set; }
        
        // Clés étrangères requises pour l'association
        [Required(ErrorMessage = "L'ID du compte est requis.")]
        public int CompteId { get; set; }
        
        [Required(ErrorMessage = "L'ID de la catégorie est requis.")]
        public int CategorieTransactionId { get; set; }
        
        [Required(ErrorMessage = "L'ID de la discipline est requis.")]
        public int DisciplineId { get; set; }
    }