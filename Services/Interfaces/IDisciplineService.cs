using ASPPorcelette.API.Models;

namespace ASPPorcelette.API.Services.Interfaces
{
    public interface IDisciplineService
    {
        Task<IEnumerable<Discipline>> GetAllDisciplinesAsync();
        Task<Discipline?> GetDisciplineByIdAsync(int id);
        Task<Discipline> GetByIdAsync( int id);
    }
}