using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class CategorieTransactionRepository : ICategorieTransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<CategorieTransaction> _dbSet;

        public CategorieTransactionRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<CategorieTransaction>();
        }

        public async Task<IEnumerable<CategorieTransaction>> GetAllAsync()
        {
            // Pas de .Include nécessaire pour ce modèle simple
            return await _dbSet.ToListAsync();
        }

        public async Task<CategorieTransaction?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<CategorieTransaction> AddAsync(CategorieTransaction categorie)
        {
            await _dbSet.AddAsync(categorie);
            await _context.SaveChangesAsync();
            return categorie;
        }

        public async Task<bool> UpdateAsync(CategorieTransaction categorie)
        {
            var existingEntity = await _dbSet.FindAsync(categorie.CategorieTransactionId);
            if (existingEntity == null)
            {
                return false;
            }
            
            // Met à jour toutes les propriétés modifiées
            _context.Entry(existingEntity).CurrentValues.SetValues(categorie);
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
