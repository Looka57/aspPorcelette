using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace ASPPorcelette.API.Repository.Implementation
{

    public class AdherentRepository : IAdherentRepository
    {
        private readonly ApplicationDbContext _context;

        public AdherentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Adherent>> GetAllAsync()
        {
            return await _context.Adherents.ToListAsync();
        }

        public async Task<Adherent?> GetByIdAsync(int id)
        {
            return await _context.Adherents.FindAsync(id);
        }

        public async Task<Adherent> AddAsync(Adherent adherent)
        {
            _context.Adherents.Add(adherent);
            await _context.SaveChangesAsync();
            return adherent;
        }

        public async Task<Adherent> UpdateAdherent(Adherent adherent)
        {
            _context.Adherents.Update(adherent);
            await _context.SaveChangesAsync();
            return adherent;
        }

        public async Task<Adherent> DeleteAdherent(int id)
        {
            var adherent = await _context.Adherents.FindAsync(id);
            if (adherent != null)
            {
                _context.Adherents.Remove(adherent);
                await _context.SaveChangesAsync();
            }
            return adherent!;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}