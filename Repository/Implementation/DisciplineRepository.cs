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
            // On s'assure de retourner la liste sans Inclure Sensei pour éviter les boucles, 
            // la configuration AddNewtonsoftJson devrait gérer le reste.
            return await _context.Disciplines.ToListAsync();
        }

        public async Task<Discipline?> GetByIdAsync(int id)
        {
            return await _context.Disciplines.FindAsync(id);
        }
    }
}
