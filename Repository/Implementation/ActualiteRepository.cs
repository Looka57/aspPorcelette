using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class ActualiteRepository : IActualiteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Actualite> _dbSet;

        public ActualiteRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Actualite>();
        }

        private IQueryable<Actualite> GetQueryWithDetails()
        {
            // Requête de base pour inclure toutes les relations de navigation
            return _dbSet
         .Include(a => a.User)               // ← navigation vers User (Sensei)
         .Include(a => a.EvenementAssocie);

        }

        // -----------------------------------------------------------------
        // Méthodes de Lecture (READ)
        // -----------------------------------------------------------------

        // Dans ActualiteRepository.cs

        public async Task<IEnumerable<Actualite>> GetAllWithDetailsAsync()
        {
            // Au lieu d'utiliser GetQueryWithDetails().ToListAsync(),
            // utilise une projection pour forcer une requête propre et garantir
            // que l'ancienne colonne SenseiId n'est pas sélectionnée.

            return await _dbSet // Utilise _dbSet au lieu de GetQueryWithDetails() temporairement
                .Include(a => a.User)
                .Include(a => a.EvenementAssocie)
                .OrderByDescending(a => a.DateDePublication)

                // 🚨 SOLUTION : FORCER UNE PROJECTION PROPRE (si le problème persiste)
                // Note: Tu devras créer une classe DTO qui ne contient pas la propriété de navigation User/Evenement
                // ou simplement retourner l'objet Actualite complet si la projection n'est pas l'objectif.
                // MAIS pour tester l'exclusion de l'ancienne colonne, essayons juste la requête sans la méthode utilitaire :

                .ToListAsync();
        }

        public async Task<Actualite?> GetByIdWithDetailsAsync(int id)
        {
            return await GetQueryWithDetails()
                .FirstOrDefaultAsync(a => a.ActualiteId == id);
        }

        public async Task<Actualite?> GetByIdAsync(int id)
        {
            // Utilisé principalement pour le PATCH/Update où seules les FK sont manipulées
            return await _dbSet.FindAsync(id);
        }

        // -----------------------------------------------------------------
        // Méthodes de Création/Mise à jour/Suppression (CUD)
        // -----------------------------------------------------------------

        public async Task<Actualite> AddAsync(Actualite actualite)
        {
            await _dbSet.AddAsync(actualite);
            await _context.SaveChangesAsync();
            return actualite;
        }

        public async Task<bool> UpdateAsync(Actualite actualite)
        {
            var existingEntity = await _dbSet.FindAsync(actualite.ActualiteId);
            if (existingEntity == null)
            {
                return false;
            }

            // Met à jour toutes les propriétés de l'entité existante avec celles de l'entité passée
            _context.Entry(existingEntity).CurrentValues.SetValues(actualite);

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
