using ASPPorcelette.API.DTOs.User;
using ASPPorcelette.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace ASPPorcelette.API.Services
{
    public interface IUserService
    {
        // Création d'utilisateur avec rôle
        Task<IdentityResult> CreateUserWithProfileAsync(
            UserCreationDto dto,
            string? role = null
        );
        // Mise à jour du profil utilisateur
        Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto updateDto);

        // 👈 NOUVELLE méthode pour la mise à jour SANS mot de passe par l'admin
        Task<IdentityResult> UpdateUserByAdminAsync(string userId, UserUpdateDto dto);
        // Liste des utilisateurs pour l'admin
        Task<IEnumerable<UserDto>> GetAdminUserListAsync();

        // Fichier : IUserService.cs
        Task<IdentityResult> DeactivateUserAsync(string userId);

        Task<IdentityResult> RenewAdhesionAsync(string userId);

        Task<int> GetActiveAdherentsCountAsync();






    }
}