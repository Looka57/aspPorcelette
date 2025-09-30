// DTO de Mise à Jour (PUT/PATCH)
using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Models;

public class CompteUpdateDto
    {
        [MaxLength(150)]
        public string? Nom { get; set; }
        
        public TypeCompte? TypeCompte { get; set; }

        // Le solde peut être mis à jour (Solde Initial du modèle)
        [Range(typeof(decimal), "-999999.99", "999999.99", ErrorMessage = "Le solde doit être une valeur monétaire valide.")]
        public decimal? Solde { get; set; }

        [Range(typeof(decimal), "0.00", "999999.99", ErrorMessage = "Le montant d'épargne doit être positif.")]
        public decimal? Epargne { get; set; }
    }