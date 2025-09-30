using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface ITarifRepository
    {
        // CRUD de base
        Task<Tarif> AddAsync(Tarif tarif);
        Task<bool> UpdateAsync(Tarif tarif);
        Task<bool> DeleteAsync(int id); 

        // Méthodes spécifiques nécessitant le chargement des relations
        Task<IEnumerable<Tarif>> GetAllTarifsWithDisciplineAsync();
        Task<Tarif?> GetTarifByIdWithDisciplineAsync(int id);
    }
}
