using ASPPorcelette.API.Models;

namespace ASPPorcelette.API.Services
{
    public interface ISenseiService
    {
        Task<IEnumerable<Sensei>> GetAllSenseisAsync();
        Task<Sensei?> GetSenseiByIdAsync(int id);
        Task<Sensei> CreateSenseiAsync(Sensei sensei);
        Task<Sensei> UpdateSenseiAsync(Sensei sensei);
        Task<Sensei> DeleteSenseiAsync(int id);
    }
}