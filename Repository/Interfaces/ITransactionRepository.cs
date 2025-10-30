using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        // Récupération de toutes les transactions avec les relations incluses
        Task<IEnumerable<Transaction>> GetAllWithDetailsAsync();

        // Récupération d'une transaction spécifique avec les relations incluses
        Task<Transaction?> GetByIdWithDetailsAsync(int id);
        
        Task<IEnumerable<Transaction>> GetByCompteIdWithDetailsAsync(int compteId);


        // Récupération d'une transaction simple par ID (utilisé pour le mapping PUT/PATCH)
        Task<Transaction?> GetByIdAsync(int id);

        // CRUD
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction?> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(int id);
    }
}
