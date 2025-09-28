using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface ICoursRepository
    {
        // CRUD de base
        Task<IEnumerable<Cours>> GetAllAsync();
        Task<Cours?> GetByIdAsync(int id);
        Task<Cours> AddAsync(Cours cours);
        Task<Cours> UpdateCours(Cours cours);
        Task<Cours> DeleteCours(int id);

        // Méthode spécifique pour charger les relations et les horaires
        Task<Cours?> GetCoursWithDetailsAsync(int id);
        Task<IEnumerable<Cours>> GetAllCoursWithDetailsAsync();

        // Méthode pour sauvegarder les changements
        bool SaveChanges();
    }
}
