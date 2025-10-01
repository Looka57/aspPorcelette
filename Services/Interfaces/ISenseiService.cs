using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs;
using Microsoft.AspNetCore.Identity;
using ASPPorcelette.DTOs;
using ASPPorcelette.API.DTOs.User;

namespace ASPPorcelette.API.Services
{
    public interface ISenseiService
    {

        Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto);
        Task<IEnumerable<Sensei>> GetAllSenseisAsync();
        Task<Sensei?> GetSenseiByIdAsync(int id);
        Task<Sensei> CreateSenseiAsync(Sensei sensei);
        Task<Sensei> UpdateSenseiAsync(Sensei sensei);
        Task<(Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(
            int id, 
            JsonPatchDocument<SenseiUpdateDto> patchDocument
        );
         Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto updateDto);         Task<Sensei> DeleteSenseiAsync(int id);
    }
}