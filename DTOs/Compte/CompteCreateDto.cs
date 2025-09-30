// DTO de Création (POST)
using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Models;

public class CompteCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string Nom { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Le type de compte (Courant ou Epargne) est requis.")]
        public TypeCompte TypeCompte { get; set; }

        // Solde requis à la création (Solde Initial du modèle)
        [Required]
        [Range(typeof(decimal), "-999999.99", "999999.99", ErrorMessage = "Le solde initial doit être une valeur monétaire valide.")]
        public decimal Solde { get; set; } = 0.00m;
        
        // Épargne requise à la création
        [Required]
        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "Le montant d'épargne doit être positif.")]
        public decimal Epargne { get; set; } = 0.00m;
    }