using ASPPorcelette.API.DTOs.Transaction;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICompteRepository _compteRepository;
        private readonly ICategorieTransactionRepository _categorieTransactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICompteRepository compteRepository,
            ICategorieTransactionRepository categorieTransactionRepository,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _compteRepository = compteRepository;
            _categorieTransactionRepository = categorieTransactionRepository;
            _mapper = mapper;
        }

        // ============================================================
        // 🔹 Récupération de toutes les transactions avec leurs détails
        // ============================================================
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllWithDetailsAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetByIdWithDetailsAsync(id);
        }

        // ============================================================
        // 🔹 Calcul du montant signé selon le TypeFlux
        // ============================================================
        private async Task<decimal> CalculateSignedMontantAsync(int categorieId, decimal montant)
        {
            var categorie = await _categorieTransactionRepository.GetByIdAsync(categorieId);
            if (categorie == null)
                throw new KeyNotFoundException($"Catégorie de transaction ID {categorieId} non trouvée.");

            decimal absoluteMontant = Math.Abs(montant);
            string fluxValue = categorie.TypeFlux.ToString()?.ToLower() ?? "";

            bool isDepense = fluxValue.Contains("2") || fluxValue.Contains("depense");

            decimal signedMontant = isDepense ? -absoluteMontant : absoluteMontant;

            Console.WriteLine($"[DEBUG] Calcul TypeFlux={categorie.TypeFlux}, MontantSaisi={montant}, MontantSigné={signedMontant}");
            return signedMontant;
        }

        // ============================================================
        // 🔹 Création d'une transaction
        // ============================================================
        public async Task<Transaction> CreateTransactionAsync(TransactionCreateDto transactionDto)
        {
            // 1️⃣ Mapper le DTO vers l'entité Transaction
            var transaction = _mapper.Map<Transaction>(transactionDto);

            // 2️⃣ Récupérer le compte concerné
            var compte = await _compteRepository.GetByIdAsync(transaction.CompteId);
            if (compte == null)
                throw new KeyNotFoundException($"Compte ID {transaction.CompteId} non trouvé.");

            // 3️⃣ Récupérer la catégorie pour connaître le type de flux
            var categorie = await _categorieTransactionRepository.GetByIdAsync(transaction.CategorieTransactionId);
            if (categorie == null)
                throw new KeyNotFoundException($"Catégorie ID {transaction.CategorieTransactionId} non trouvée.");

            // 4️⃣ Déterminer le signe du montant
            decimal montantBase = Math.Abs(transaction.Montant);
            string fluxValue = categorie.TypeFlux.ToString()?.ToLower() ?? "";
            bool isDepense = fluxValue.Contains("2") || fluxValue.Contains("depense");

            decimal signedMontant = isDepense ? -montantBase : montantBase;

            // 🧩 Log de debug
            Console.WriteLine($"[DEBUG TRANSACTION CREATE] Catégorie={categorie.Nom}, TypeFlux={categorie.TypeFlux}, " +
             $"MontantSaisi={transaction.Montant}, MontantSigné={signedMontant}, CompteAvant={compte.Solde}");

            // 🧮 Affecter le montant signé
            transaction.Montant = signedMontant;

            // 5️⃣ Mettre à jour le solde du compte
            compte.Solde += signedMontant;
            await _compteRepository.UpdateCompteAsync(compte);

            // 6️⃣ Ajouter la transaction
            var createdTransaction = await _transactionRepository.AddAsync(transaction);

            // 7️⃣ Log du résultat
            Console.WriteLine($"[DEBUG TRANSACTION RESULT] Nouveau solde={compte.Solde}, TransactionID={createdTransaction.TransactionId}");

            // 8️⃣ Retourner la transaction complète avec ses détails
            return await _transactionRepository.GetByIdWithDetailsAsync(createdTransaction.TransactionId);
        }

// -----------------------------------------------------------------
// Récupérer les 5 dernières transactions (triées par date décroissante)
// -----------------------------------------------------------------
public async Task<IEnumerable<Transaction>> GetLast5TransactionsAsync()
{
    var allTransactions = await _transactionRepository.GetAllWithDetailsAsync();
    return allTransactions
        .OrderByDescending(t => t.DateTransaction)
        .Take(5);
}
























        // ============================================================
        // 🔹 Mise à jour complète (PUT)
        // ============================================================
        public async Task<Transaction?> UpdateTransactionAsync(int id, TransactionUpdateDto transactionDto)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null)
                return null;

            var oldMontant = existingTransaction.Montant;
            var oldCompteId = existingTransaction.CompteId;

            _mapper.Map(transactionDto, existingTransaction);
            existingTransaction.TransactionId = id;

            var newSignedMontant = await CalculateSignedMontantAsync(
                existingTransaction.CategorieTransactionId,
                transactionDto.Montant ?? 0m
            );
            existingTransaction.Montant = newSignedMontant;

            // Correction des soldes
            if (oldCompteId != existingTransaction.CompteId || oldMontant != newSignedMontant)
            {
                var oldCompte = await _compteRepository.GetByIdAsync(oldCompteId);
                if (oldCompte != null)
                {
                    oldCompte.Solde -= oldMontant;
                    await _compteRepository.UpdateCompteAsync(oldCompte);
                }

                var newCompte = await _compteRepository.GetByIdAsync(existingTransaction.CompteId);
                if (newCompte == null)
                    throw new KeyNotFoundException($"Nouveau Compte ID {existingTransaction.CompteId} non trouvé.");

                newCompte.Solde += newSignedMontant;
                await _compteRepository.UpdateCompteAsync(newCompte);
            }

            await _transactionRepository.UpdateAsync(existingTransaction);

            return await _transactionRepository.GetByIdWithDetailsAsync(id);
        }

        // ============================================================
        // 🔹 Mise à jour partielle (PATCH)
        // ============================================================
        public async Task<(Transaction? Transaction, bool Success)> PartialUpdateTransactionAsync(
            int id,
            JsonPatchDocument<TransactionUpdateDto> patchDocument)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null)
                return (null, false);

            var oldMontant = existingTransaction.Montant;
            var oldCompteId = existingTransaction.CompteId;

            var transactionDtoToPatch = _mapper.Map<TransactionUpdateDto>(existingTransaction);
            patchDocument.ApplyTo(transactionDtoToPatch);
            _mapper.Map(transactionDtoToPatch, existingTransaction);

            var newSignedMontant = await CalculateSignedMontantAsync(
                existingTransaction.CategorieTransactionId,
                transactionDtoToPatch.Montant ?? 0m
            );
            existingTransaction.Montant = newSignedMontant;

            if (oldCompteId != existingTransaction.CompteId || oldMontant != newSignedMontant)
            {
                var oldCompte = await _compteRepository.GetByIdAsync(oldCompteId);
                if (oldCompte != null)
                {
                    oldCompte.Solde -= oldMontant;
                    await _compteRepository.UpdateCompteAsync(oldCompte);
                }

                var newCompte = await _compteRepository.GetByIdAsync(existingTransaction.CompteId);
                if (newCompte == null)
                    return (null, false);

                newCompte.Solde += newSignedMontant;
                await _compteRepository.UpdateCompteAsync(newCompte);
            }

            await _transactionRepository.UpdateAsync(existingTransaction);

            var transactionWithDetails = await _transactionRepository.GetByIdWithDetailsAsync(existingTransaction.TransactionId);
            return (transactionWithDetails, true);
        }

        // ============================================================
        // 🔹 Suppression
        // ============================================================
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null)
                return false;

            var compte = await _compteRepository.GetByIdAsync(transaction.CompteId);
            if (compte != null)
            {
                compte.Solde -= transaction.Montant;
                await _compteRepository.UpdateCompteAsync(compte);
            }

            return await _transactionRepository.DeleteAsync(id);
        }
    }
}
