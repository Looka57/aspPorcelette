using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Implementation
{
    public class CompteRepository : ICompteRepository
    {
        private readonly ApplicationDbContext _context;

        public CompteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Compte>> GetAllAsync()
        {
            // Correction : Inclure Transactions au lieu de TransactionsEffectuees
            return await _context.Comptes
                                 .Include(c => c.Transactions) 
                                 .ToListAsync();
        }

        public async Task<Compte?> GetByIdAsync(int id)
        {
            // Correction : Inclure Transactions au lieu de TransactionsEffectuees
            return await _context.Comptes
                                 .Include(c => c.Transactions) 
                                 .FirstOrDefaultAsync(c => c.CompteId == id);
        }

        public async Task<Compte> AddAsync(Compte compte)
        {
            _context.Comptes.Add(compte);
            await _context.SaveChangesAsync();
            return compte;
        }

        public async Task<bool> UpdateAsync(Compte compte)
        {
            _context.Comptes.Update(compte);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Compte> UpdateCompteAsync(Compte compte)
        {
            _context.Comptes.Update(compte);
            await _context.SaveChangesAsync();
            return compte;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var compte = await _context.Comptes.FindAsync(id);
            if (compte == null)
            {
                return false;
            }

            _context.Comptes.Remove(compte);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
