using ASPPorcelette.API.DTOs.Transaction;
using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> GetTransactionByIdAsync(int id);

        Task<IEnumerable<Transaction>> GetTransactionsByCompteIdAsync(int compteId);

        // Création avec logique de mise à jour du solde du compte
        Task<Transaction> CreateTransactionAsync(TransactionCreateDto transactionDto, Guid connectedUserId);

        // Mise à jour (PUT) avec logique de mise à jour du solde du compte
        Task<Transaction?> UpdateTransactionAsync(int id, TransactionUpdateDto transactionDto);

        // Mise à jour partielle (PATCH)
        Task<(Transaction? Transaction, bool Success)> PartialUpdateTransactionAsync(
            int id,
            JsonPatchDocument<TransactionUpdateDto> patchDocument
        );


        // Suppression avec logique de correction du solde du compte
        Task<bool> DeleteTransactionAsync(int id);
        Task TransferAsync(int sourceId, int destId, decimal montant, string description, int? categorieId, int disciplineId, Guid UserId);
    }
}
