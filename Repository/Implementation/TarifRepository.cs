using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class TarifRepository : ITarifRepository
    {
        private readonly ApplicationDbContext _context;

        public TarifRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Requête de base pour inclure la relation Discipline
        private IQueryable<Tarif> GetQueryWithDiscipline()
        {
            return _context.Tarifs.Include(t => t.Discipline);
        }

        // --- Méthodes avec chargement des relations ---
        
        public async Task<IEnumerable<Tarif>> GetAllTarifsWithDisciplineAsync()
        {
            return await GetQueryWithDiscipline().ToListAsync();
        }

        public async Task<Tarif?> GetTarifByIdWithDisciplineAsync(int id)
        {
            return await GetQueryWithDiscipline().FirstOrDefaultAsync(t => t.TarifId == id);
        }

        // --- CRUD de base ---

        public async Task<Tarif> AddAsync(Tarif tarif)
        {
            await _context.Tarifs.AddAsync(tarif);
            await _context.SaveChangesAsync();
            return tarif;
        }

        public async Task<bool> UpdateAsync(Tarif tarif)
        {
            _context.Tarifs.Update(tarif);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tarif = await _context.Tarifs.FindAsync(id);
            if (tarif == null)
            {
                return false;
            }
            _context.Tarifs.Remove(tarif);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
