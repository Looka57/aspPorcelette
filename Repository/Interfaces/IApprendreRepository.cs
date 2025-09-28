using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    // Interface spécifique pour la gestion de la clé composite 'Apprendre'
    public interface IApprendreRepository
    {
        // CRUD de base (avec clé composite pour GET et DELETE)
        Task<Apprendre> AddAsync(Apprendre entity);

        // Récupération de toutes les inscriptions avec les relations (chargement inclus)
        Task<IEnumerable<Apprendre>> GetAllWithRelationsAsync();

        // Récupération par clé composite (les deux IDs sont nécessaires)
        Task<Apprendre?> GetByIdsAsync(int adherentId, int disciplineId);
        
        // Récupération par AdherentId
        Task<IEnumerable<Apprendre>> GetByAdherentIdAsync(int adherentId);

        // Suppression par clé composite (les deux IDs sont nécessaires)
        Task<bool> DeleteAsync(int adherentId, int disciplineId);
    }
}
