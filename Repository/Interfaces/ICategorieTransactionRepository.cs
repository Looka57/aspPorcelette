using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface ICategorieTransactionRepository
    {
        Task<IEnumerable<CategorieTransaction>> GetAllAsync();
        Task<CategorieTransaction?> GetByIdAsync(int id);
        Task<CategorieTransaction> AddAsync(CategorieTransaction categorie);
        Task<bool> UpdateAsync(CategorieTransaction categorie);
        Task<bool> DeleteAsync(int id);
    }
}
