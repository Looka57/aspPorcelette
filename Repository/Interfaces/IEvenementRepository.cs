using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface IEvenementRepository
    {
        // CRUD de base
        Task<IEnumerable<Evenement>> GetAllAsync();
        Task<Evenement?> GetByIdAsync(int id);
        Task<Evenement> AddAsync(Evenement evenement);
        Task<bool> UpdateAsync(Evenement evenement);
        Task<bool> DeleteAsync(int id);

        // Méthodes spécifiques avec chargement des relations
        Task<Evenement?> GetEvenementWithDetailsAsync(int id);
        Task<IEnumerable<Evenement>> GetAllEvenementsWithDetailsAsync();
    }
}
