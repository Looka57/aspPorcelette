using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class EvenementRepository : IEvenementRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Evenement> _dbSet;

        public EvenementRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Evenement>();
        }
        
        // -----------------------------------------------------------------
        // Chargement avec détails (Inclusions pour les DTOs)
        // -----------------------------------------------------------------

        public async Task<IEnumerable<Evenement>> GetAllEvenementsWithDetailsAsync()
        {
            return await _dbSet
                // Charger le type d'événement (obligatoire pour le DTO de réponse)
                .Include(e => e.TypeEvenement)
                // Charger la discipline (optionnel)
                .Include(e => e.Discipline)
                .ToListAsync();
        }

        public async Task<Evenement?> GetEvenementWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(e => e.TypeEvenement)
                .Include(e => e.Discipline)
                .FirstOrDefaultAsync(e => e.EvenementId == id);
        }

        // -----------------------------------------------------------------
        // CRUD de base (sans chargement des relations)
        // -----------------------------------------------------------------
        
        public async Task<IEnumerable<Evenement>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Evenement?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Evenement> AddAsync(Evenement evenement)
        {
            await _dbSet.AddAsync(evenement);
            await _context.SaveChangesAsync();
            return evenement; // L'entité contient maintenant l'ID
        }

        public async Task<bool> UpdateAsync(Evenement evenement)
        {
            var existingEntity = await _dbSet.FindAsync(evenement.EvenementId);
            if (existingEntity == null)
            {
                return false;
            }

            // Met à jour les valeurs de l'entité existante avec celles passées
            _context.Entry(existingEntity).CurrentValues.SetValues(evenement);
            
            // Assurez-vous que les clés étrangères sont mises à jour si elles sont nulles/omises dans SetValues
            existingEntity.TypeEvenementId = evenement.TypeEvenementId;
            existingEntity.DisciplineId = evenement.DisciplineId;

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
