// Fichier : Repository/Interfaces/IHoraireRepository.cs
using ASPPorcelette.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface IHoraireRepository
    {
        // CRUD de base
        Task<IEnumerable<Horaire>> GetAllAsync();
        Task<Horaire?> GetByIdAsync(int id);
        // Task<Horaire> AddAsync(Horaire horaire);
        // Task<Horaire?> UpdateAsync(Horaire horaire);
        // Task<bool> DeleteAsync(int id);

        // Méthodes spécifiques (avec relations)
        Task<IEnumerable<Horaire>> GetAllWithCoursAsync();
        Task<Horaire?> GetByIdWithCoursAsync(int id);
    }
}