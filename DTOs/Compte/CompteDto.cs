using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Models; // Pour utiliser TypeCompte

namespace ASPPorcelette.API.DTOs.Compte
{
    // DTO de Réponse (GET)
    public class CompteDto
    {
        public int CompteId { get; set; }
        public string Nom { get; set; } = string.Empty;

        // Type de compte
        public TypeCompte TypeCompte { get; set; }

        // Solde initial (stocké en DB)
        public decimal Solde { get; set; }

        // Montant d'épargne (stocké en DB)
        public decimal Epargne { get; set; }

        // Solde Actuel CALCULÉ par le Service
        public decimal SoldeActuel { get; set; }
    }
}