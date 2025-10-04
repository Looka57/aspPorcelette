using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs.User; // Assurez-vous que ce using est correct
using Microsoft.AspNetCore.Identity;
using ASPPorcelette.DTOs;
using ASPPorcelette.API.DTOs; // À vérifier si ce DTO est toujours nécessaire

namespace ASPPorcelette.API.Services
{
    public interface ISenseiService
    {
        // CONSERVÉE (La bonne signature pour l'inscription)
        Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto);
        
        // CONSERVÉE (La bonne signature pour la mise à jour de profil)
        Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto updateDto);

        // --- Méthodes spécifiques à l'entité Sensei (pour l'Admin) ---
        
        Task<IEnumerable<Sensei>> GetAllSenseisAsync();
        Task<Sensei?> GetSenseiByIdAsync(int id);
        Task<Sensei> CreateSenseiAsync(Sensei sensei);
        Task<Sensei> UpdateSenseiAsync(Sensei sensei);
        Task<(Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(
            int id, 
            JsonPatchDocument<SenseiUpdateDto> patchDocument
        );
        Task<Sensei> DeleteSenseiAsync(int id);
        
        // L'ancienne ligne "Task CreateUserWithProfileAsync(object userCreateDto);" 
        // a été supprimée.
    }
}
