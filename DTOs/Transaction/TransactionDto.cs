using System;
using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.DTOs.CategorieTransaction;
using ASPPorcelette.API.DTOs.Compte; // Pour CompteDto
using ASPPorcelette.API.DTOs.Discipline; // Pour DisciplineDto

namespace ASPPorcelette.API.DTOs.Transaction
{
    // DTO de Réponse (GET)
    // Affiche la transaction avec les détails des entités liées (Compte, Catégorie, Discipline)
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public DateTimeOffset DateTransaction { get; set; }
        
        // Montant peut être positif ou négatif
        public decimal Montant { get; set; } 
        public string? Description { get; set; }

        // Détails des entités liées (utilisons des DTOs légers pour éviter les boucles)
        public CompteDto Compte { get; set; } 
        public CategorieTransactionDto Categorie { get; set; } 
        public DisciplineDto Discipline { get; set; } 
    }

    


}
