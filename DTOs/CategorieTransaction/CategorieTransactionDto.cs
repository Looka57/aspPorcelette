using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Enums;

namespace ASPPorcelette.API.DTOs.CategorieTransaction
{
    // DTO de Réponse (GET)
    public class CategorieTransactionDto
    {
        public int CategorieTransactionId { get; set; }
        public string Nom { get; set; } = string.Empty;
        // Utilisation de l'enum
        public TypeFlux TypeFlux { get; set; }
    }

}
