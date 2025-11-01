using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly ApplicationDbContext _context;

        public DisciplineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            return await _context.Disciplines
                .Include(d => d.CoursAssocies)
                    .ThenInclude(c => c.Horaires) // 🔹 Charge les horaires du cours
                .Include(d => d.CoursAssocies)
                    .ThenInclude(c => c.User) // Charge le sensei
                .ToListAsync();
        }

        public async Task<Discipline?> GetByIdAsync(int id)
        {
            return await _context.Disciplines
                .Include(d => d.CoursAssocies)
                    .ThenInclude(c => c.Horaires)
                .Include(d => d.CoursAssocies)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(d => d.DisciplineId == id);
        }

    }
}
