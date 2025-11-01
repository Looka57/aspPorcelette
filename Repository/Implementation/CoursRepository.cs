using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class CoursRepository : ICoursRepository
    {
        private readonly ApplicationDbContext _context;

        public CoursRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // -----------------------------------------------------------------
        // Méthodes avec chargement des relations (Sensei, Discipline, Horaires)
        // -----------------------------------------------------------------

        public async Task<IEnumerable<Cours>> GetAllCoursWithDetailsAsync()
        {
            return await _context.Cours
                // Chargement des relations N-1
                .Include(c => c.User)
                .Include(c => c.Discipline)
                // Chargement des relations 1-N (Horaires)
                .Include(c => c.Horaires)
                .ToListAsync();
        }

        public async Task<Cours?> GetCoursWithDetailsAsync(int id)
        {
            return await _context.Cours
                // Chargement des relations N-1
                .Include(c => c.User)
                .Include(c => c.Discipline)
                // Chargement des relations 1-N (Horaires)
                .Include(c => c.Horaires)
                .FirstOrDefaultAsync(c => c.CoursId == id);
        }

        // -----------------------------------------------------------------
        // CRUD de base
        // -----------------------------------------------------------------

        public async Task<IEnumerable<Cours>> GetAllAsync()
        {
            // Simple GetAll pour les cas où les détails ne sont pas nécessaires
            return await _context.Cours.ToListAsync();
        }

        public async Task<Cours?> GetByIdAsync(int id)
        {
            // Utilise FindAsync qui est plus rapide pour la PK simple
            return await _context.Cours.FindAsync(id);
        }

        public async Task<Cours> AddAsync(Cours cours)
        {
            _context.Cours.Add(cours);
            await _context.SaveChangesAsync();
            // L'entité 'cours' contient maintenant l'ID généré
            return cours;
        }

        public async Task<Cours> UpdateCours(Cours cours)
        {
            _context.Cours.Update(cours);
            await _context.SaveChangesAsync();
            return cours;
        }

        public async Task<Cours> DeleteCours(int id)
        {
            var cours = await _context.Cours.FindAsync(id);
            if (cours != null)
            {
                _context.Cours.Remove(cours);
                await _context.SaveChangesAsync();
            }
            return cours!;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
