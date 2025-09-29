using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface IActualiteRepository
    {
        // CRUD de base
        Task<Actualite> AddAsync(Actualite actualite);
        Task<bool> UpdateAsync(Actualite actualite);
        Task<bool> DeleteAsync(int id);
        
        // Méthodes de lecture avec chargement des relations (Sensei et Discipline)
        Task<Actualite?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Actualite>> GetAllWithDetailsAsync();

        // Simple GetById pour l'initialisation du Patch/Update
        Task<Actualite?> GetByIdAsync(int id); 
    }
}
