using System;
using ASPPorcelette.API.DTOs.CategorieTransaction;
using ASPPorcelette.API.DTOs.Compte;
using ASPPorcelette.API.DTOs.Discipline;

namespace ASPPorcelette.API.DTOs.Transaction
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public DateTimeOffset DateTransaction { get; set; }
        public decimal Montant { get; set; }
        public string? Description { get; set; }

        // Détails liés
        public CompteDto Compte { get; set; }
        public CategorieTransactionDto Categorie { get; set; }
        public DisciplineDto Discipline { get; set; }
    }
}
