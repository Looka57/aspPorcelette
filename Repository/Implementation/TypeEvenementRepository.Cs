// Fichier : Repository/Implementation/TypeEvenementRepository.cs
using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class TypeEvenementRepository : ITypeEvenementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TypeEvenement> _dbSet;

        public TypeEvenementRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TypeEvenement>();
        }

        public async Task<IEnumerable<TypeEvenement>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TypeEvenement?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TypeEvenement> AddAsync(TypeEvenement typeEvenement)
        {
            await _dbSet.AddAsync(typeEvenement);
            await _context.SaveChangesAsync();
            return typeEvenement;
        }

        public async Task<TypeEvenement?> UpdateAsync(TypeEvenement typeEvenement)
        {
            var existingEntity = await _dbSet.FindAsync(typeEvenement.TypeEvenementId);
            if (existingEntity == null) return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(typeEvenement);
            await _context.SaveChangesAsync();
            return existingEntity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}