using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Requête de base pour inclure les trois relations nécessaires au DTO de réponse
        private IQueryable<Transaction> GetQueryWithDetails()
        {
            return _context.Transactions
                .Include(t => t.Compte)          // Inclut le Compte
                .Include(t => t.Categorie)      // Inclut la CategorieTransaction
                .Include(t => t.Discipline);    // Inclut la Discipline
        }

        public async Task<IEnumerable<Transaction>> GetAllWithDetailsAsync()
        {
            // Récupère toutes les transactions avec leurs détails
            return await GetQueryWithDetails()
                .OrderByDescending(t => t.DateTransaction) // Tri par date récente
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdWithDetailsAsync(int id)
        {
            // Récupère une transaction par ID avec ses détails
            return await GetQueryWithDetails()
                .FirstOrDefaultAsync(t => t.TransactionId == id);
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            // Récupère la transaction sans charger les relations (utile pour le mapping PATCH/PUT)
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<IEnumerable<Transaction>> GetByCompteIdWithDetailsAsync(int compteId)
        {
            return await GetQueryWithDetails()
                .Where(t => t.CompteId == compteId)
                .OrderByDescending(t => t.DateTransaction)
                .ToListAsync();
        }


        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            // Assurez-vous que l'entité est attachée et que les valeurs sont mises à jour
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return false;
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
