using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs;

namespace ASPPorcelette.API.Services
{
    public interface ISenseiService
    {
        Task<IEnumerable<Sensei>> GetAllSenseisAsync();
        Task<Sensei?> GetSenseiByIdAsync(int id);
        Task<Sensei> CreateSenseiAsync(Sensei sensei);
        Task<Sensei> UpdateSenseiAsync(Sensei sensei);
        Task<(Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(
            int id, 
            JsonPatchDocument<SenseiUpdateDto> patchDocument
        );
        Task<Sensei> DeleteSenseiAsync(int id);
    }
}