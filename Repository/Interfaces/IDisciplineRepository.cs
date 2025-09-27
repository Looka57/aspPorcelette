using ASPPorcelette.API.Models;

namespace ASPPorcelette.API.Repository.Interfaces
{
    public interface IDisciplineRepository
    {
        Task<IEnumerable<Discipline>> GetAllAsync(); // Récupère toutes les Disciplines
        Task<Discipline?> GetByIdAsync(int id); // Récupère une Discipline par son ID
    }
}