using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    // Implémente l'interface spécifique IApprendreRepository
    public class ApprendreRepository : IApprendreRepository 
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Apprendre> _dbSet; 

        public ApprendreRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Apprendre>(); 
        }

        // Requête de base pour inclure les relations nécessaires (Adherent et Discipline)
        private IQueryable<Apprendre> GetQueryWithRelations()
        {
            return _dbSet
                .Include(a => a.AdherentApprenant)
                .Include(a => a.DisciplinePratiquee);
        }

        // GET ALL: Récupère toutes les inscriptions avec les relations
        public async Task<IEnumerable<Apprendre>> GetAllWithRelationsAsync()
        {
            return await GetQueryWithRelations().ToListAsync();
        }

        // GET BY KEYS: Récupère une inscription par sa clé composite
        public async Task<Apprendre?> GetByIdsAsync(int adherentId, int disciplineId)
        {
            return await GetQueryWithRelations()
                .FirstOrDefaultAsync(a => a.AdherentId == adherentId && a.DisciplineId == disciplineId);
        }

        // GET BY ADHERENT ID: Récupère toutes les disciplines apprises par un adhérent
        public async Task<IEnumerable<Apprendre>> GetByAdherentIdAsync(int adherentId)
        {
            return await GetQueryWithRelations()
                .Where(a => a.AdherentId == adherentId)
                .ToListAsync();
        }

        // POST: Ajoute une nouvelle inscription
        public async Task<Apprendre> AddAsync(Apprendre entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // DELETE: Supprime une inscription par sa clé composite
        public async Task<bool> DeleteAsync(int adherentId, int disciplineId)
        {
            // 1. Trouver l'entité par sa clé composite
            var entityToDelete = await _dbSet.FindAsync(adherentId, disciplineId);

            if (entityToDelete == null)
            {
                return false;
            }

            // 2. Supprimer l'entité
            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
