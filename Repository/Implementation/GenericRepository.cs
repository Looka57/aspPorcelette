using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASPPorcelette.API.Repository.Interfaces;
using System.Linq;

namespace ASPPorcelette.API.Repository.Implementation
{
    // T est le type d'entité (ex: Sensei, Adherent, Cours)
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // Remplacer 'Data.ApplicationDbContext' par le nom réel de votre DbContext si différent
        protected readonly Data.ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(Data.ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        // -----------------------------------------------------------------
        // Méthodes de LECTURE
        // -----------------------------------------------------------------

        // Récupère toutes les entités de ce type
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        // Récupère une entité par son ID
        public async Task<T?> GetByIdAsync(int id)
        {
            // La méthode FindAsync est optimisée pour la recherche par clé primaire
            return await _dbSet.FindAsync(id);
        }

        // -----------------------------------------------------------------
        // Méthodes d'ÉCRITURE
        // -----------------------------------------------------------------

        // Ajout d'une nouvelle entité
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Mise à jour d'une entité existante
        // La méthode retourne Task<bool> pour indiquer le succès ou l'échec
        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                // Attache l'entité et marque l'état comme Modified
                _dbSet.Update(entity);
                
                // Sauvegarde les changements
                var changes = await _context.SaveChangesAsync();
                
                // Retourne vrai si au moins une ligne a été affectée
                return changes > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Gérer les erreurs de concurrence si nécessaire
                return false;
            }
        }

        // Suppression d'une entité par l'OBJET (requis par l'interface)
        public async Task<bool> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }
        
        // Suppression d'une entité par son ID (méthode de commodité)
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbSet.Remove(entity);
            var changes = await _context.SaveChangesAsync();
            
            return changes > 0;
        }
    }
}
