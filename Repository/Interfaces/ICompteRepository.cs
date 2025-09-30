using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface ICompteRepository
    {
        Task<IEnumerable<Compte>> GetAllAsync();
        Task<Compte?> GetByIdAsync(int id);
        Task<Compte> AddAsync(Compte compte);
        Task<Compte>UpdateCompteAsync(Compte compte);
        Task<bool> UpdateAsync(Compte compte);
        Task<bool> DeleteAsync(int id);
    }
}
