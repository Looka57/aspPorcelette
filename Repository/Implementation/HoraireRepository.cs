// Fichier : Repository/Implementation/HoraireRepository.cs
using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class HoraireRepository : IHoraireRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Horaire> _dbSet;

        public HoraireRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Horaire>();
        }

        // Requête de base avec les relations
        private IQueryable<Horaire> GetQueryWithCours()
        {
            return _dbSet.Include(h => h.Cours);
        }

        // --- Méthodes CRUD de base sans relations ---
        public async Task<IEnumerable<Horaire>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Horaire?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // public async Task<Horaire> AddAsync(Horaire horaire)
        // {
        //     await _dbSet.AddAsync(horaire);
        //     await _context.SaveChangesAsync();
        //     return horaire;
        // }

        // public async Task<Horaire?> UpdateAsync(Horaire horaire)
        // {
        //     var existingHoraire = await _dbSet.FindAsync(horaire.HoraireId);
        //     if (existingHoraire == null) return null;

        //     _context.Entry(existingHoraire).CurrentValues.SetValues(horaire);
        //     await _context.SaveChangesAsync();
        //     return existingHoraire;
        // }

        // public async Task<bool> DeleteAsync(int id)
        // {
        //     var horaire = await _dbSet.FindAsync(id);
        //     if (horaire == null) return false;

        //     _dbSet.Remove(horaire);
        //     await _context.SaveChangesAsync();
        //     return true;
        // }

        // --- Méthodes spécifiques avec relations ---

        public async Task<IEnumerable<Horaire>> GetAllWithCoursAsync()
        {
            return await GetQueryWithCours().ToListAsync();
        }

        public async Task<Horaire?> GetByIdWithCoursAsync(int id)
        {
            return await GetQueryWithCours().FirstOrDefaultAsync(h => h.HoraireId == id);
        }
    }
}