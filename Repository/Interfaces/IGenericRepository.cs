using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    // Interface générique pour les opérations de base sur n'importe quel modèle (T)
    // où T doit être une classe de modèle (ex: Sensei, Adherent, Apprendre)
    public interface IGenericRepository<T> where T : class
    {
        // CRUD READ
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        // CRUD CREATE
        Task<T> AddAsync(T entity);

        // CRUD UPDATE
        Task<bool> UpdateAsync(T entity);

        // CRUD DELETE
        Task<bool> DeleteAsync(T entity);
        
        
        
    }
}
