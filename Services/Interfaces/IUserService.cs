using ASPPorcelette.API.DTOs.User;
using ASPPorcelette.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ASPPorcelette.API.Services
{
    public interface IUserService
    {
        // Création d'utilisateur avec rôle
        Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto, string role);

        // Mise à jour du profil utilisateur
        Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto updateDto);

        // Liste des utilisateurs pour l'admin
        Task<IEnumerable<UserDto>> GetAdminUserListAsync();

        // ❌ SUPPRIMEZ toutes les méthodes liées à Sensei :
        // Task<IEnumerable<Sensei>> GetAllSenseisAsync();
        // Task<Sensei?> GetSenseiByIdAsync(int id);
        // Task<Sensei> CreateSenseiAsync(Sensei sensei);
        // Task<(Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(int id, JsonPatchDocument<SenseiUpdateDto> patchDocument);
        // Task<Sensei> DeleteSenseiAsync(int id);
    }
}