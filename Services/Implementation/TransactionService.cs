using ASPPorcelette.API.DTOs.Transaction;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICompteRepository _compteRepository;
        private readonly IMapper _mapper;

        public TransactionService(
            ITransactionRepository transactionRepository, 
            ICompteRepository compteRepository, 
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _compteRepository = compteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllWithDetailsAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetByIdWithDetailsAsync(id);
        }

        // -----------------------------------------------------------------
        // Logique métier : Création
        // -----------------------------------------------------------------
        public async Task<Transaction> CreateTransactionAsync(TransactionCreateDto transactionDto)
        {
            // 1. Mapping DTO -> Entité
            var transaction = _mapper.Map<Transaction>(transactionDto);

            // 2. Trouver le Compte
            var compte = await _compteRepository.GetByIdAsync(transaction.CompteId);
            if (compte == null)
            {
                // Dans un vrai service, on lancerait une exception ici
                throw new KeyNotFoundException($"Compte ID {transaction.CompteId} non trouvé.");
            }

            // 3. Mettre à jour le solde du Compte
            compte.Solde += transaction.Montant;
            await _compteRepository.UpdateCompteAsync(compte);

            // 4. Ajouter la Transaction
            return await _transactionRepository.AddAsync(transaction);
        }

        // -----------------------------------------------------------------
        // Logique métier : Mise à jour complète (PUT)
        // -----------------------------------------------------------------
        public async Task<Transaction?> UpdateTransactionAsync(int id, TransactionUpdateDto transactionDto)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null)
            {
                return null;
            }

            var oldMontant = existingTransaction.Montant;
            var oldCompteId = existingTransaction.CompteId;

            // 1. Mappe le DTO sur l'entité existante (met à jour Montant, CompteId, etc.)
            _mapper.Map(transactionDto, existingTransaction);
            existingTransaction.TransactionId = id; // Assurer que l'ID n'est pas écrasé

            // 2. Logique de Correction du Solde
            
            // Si le Compte ou le Montant a changé, nous devons effectuer une correction.
            if (oldCompteId != existingTransaction.CompteId || oldMontant != existingTransaction.Montant)
            {
                // A. Rétablir l'ancien solde (annuler l'impact de l'ancienne transaction)
                var oldCompte = await _compteRepository.GetByIdAsync(oldCompteId);
                if (oldCompte != null)
                {
                    oldCompte.Solde -= oldMontant;
                    await _compteRepository.UpdateCompteAsync(oldCompte);
                }

                // B. Appliquer le nouveau montant au bon compte
                var newCompte = await _compteRepository.GetByIdAsync(existingTransaction.CompteId);
                if (newCompte == null)
                {
                    // Lancer une exception ou annuler l'opération si le nouveau compte n'existe pas
                    throw new KeyNotFoundException($"Nouveau Compte ID {existingTransaction.CompteId} non trouvé.");
                }
                
                newCompte.Solde += existingTransaction.Montant;
                await _compteRepository.UpdateCompteAsync(newCompte);
            }
            
            // 3. Mettre à jour la transaction dans la base de données
            var updatedTransaction = await _transactionRepository.UpdateAsync(existingTransaction);

            // 4. Retourner la transaction avec ses détails
            return await _transactionRepository.GetByIdWithDetailsAsync(id);
        }

        // -----------------------------------------------------------------
        // Logique métier : Mise à jour partielle (PATCH)
        // -----------------------------------------------------------------
        public async Task<(Transaction? Transaction, bool Success)> PartialUpdateTransactionAsync(
            int id, 
            JsonPatchDocument<TransactionUpdateDto> patchDocument)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null)
            {
                return (null, false);
            }
            
            var oldMontant = existingTransaction.Montant;
            var oldCompteId = existingTransaction.CompteId;

            var transactionDtoToPatch = _mapper.Map<TransactionUpdateDto>(existingTransaction);
            patchDocument.ApplyTo(transactionDtoToPatch);

            // Mappe le DTO patché sur l'entité existante
            _mapper.Map(transactionDtoToPatch, existingTransaction);
            
            // La logique de correction du solde doit être exécutée
            if (oldCompteId != existingTransaction.CompteId || oldMontant != existingTransaction.Montant)
            {
                // A. Rétablir l'ancien solde
                var oldCompte = await _compteRepository.GetByIdAsync(oldCompteId);
                if (oldCompte != null)
                {
                    oldCompte.Solde -= oldMontant;
                    await _compteRepository.UpdateCompteAsync(oldCompte);
                }
                
                // B. Appliquer le nouveau montant au bon compte
                var newCompte = await _compteRepository.GetByIdAsync(existingTransaction.CompteId);
                if (newCompte == null)
                {
                    // La transaction est invalide, on ne sauvegarde pas l'update
                    return (null, false);
                }
                
                newCompte.Solde += existingTransaction.Montant;
                await _compteRepository.UpdateCompteAsync(newCompte);
            }
            
            var updatedTransaction = await _transactionRepository.UpdateAsync(existingTransaction);
            
            // Retourner la transaction avec ses détails
            var transactionWithDetails = await _transactionRepository.GetByIdWithDetailsAsync(updatedTransaction!.TransactionId);
            
            return (transactionWithDetails, true);
        }
        
        // -----------------------------------------------------------------
        // Logique métier : Suppression
        // -----------------------------------------------------------------
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
            {
                return false;
            }

            // 1. Trouver le Compte
            var compte = await _compteRepository.GetByIdAsync(transaction.CompteId);
            
            // 2. Annuler l'impact de la transaction sur le solde
            if (compte != null)
            {
                // Si le montant était +50, on fait -50. Si c'était -20, on fait +20.
                compte.Solde -= transaction.Montant; 
                await _compteRepository.UpdateCompteAsync(compte);
            }

            // 3. Supprimer la Transaction
            return await _transactionRepository.DeleteAsync(id);
        }
    }
}
