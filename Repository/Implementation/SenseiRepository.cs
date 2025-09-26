using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class SenseiRepository : ISenseiRepository
    {
        private readonly ApplicationDbContext _context;

        public SenseiRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Inclut la Discipline Principale pour éviter une requête séparée
        public async Task<IEnumerable<Sensei>> GetAllAsync()
        {
            return await _context.Senseis
            .Include(s => s.DisciplinePrincipale)
            .ToListAsync();
        }

        public async Task<Sensei?> GetByIdAsync(int id)
        {
            return await _context.Senseis
            .Include(s => s.DisciplinePrincipale)
            .FirstOrDefaultAsync(s => s.SenseiId == id);
        }

        public async Task<Sensei> AddAsync(Sensei sensei)
        {
            await _context.Senseis.AddAsync(sensei);
            await _context.SaveChangesAsync();
            return sensei;
        }

        public async Task<Sensei> UpdateSensei(Sensei sensei)
        {
            _context.Senseis.Update(sensei);
            await _context.SaveChangesAsync();
            return sensei;
        }

        public async Task<Sensei> DeleteSensei(int id)
        {
            var sensei = await _context.Senseis.FindAsync(id);
            if (sensei != null)
            {
                _context.Senseis.Remove(sensei);
                await _context.SaveChangesAsync();
            }
            return sensei!;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}