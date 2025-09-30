using ASPPorcelette.API.Models.Identity;

namespace ASPPorcelette.API.Services.Identity
{
    // Interface pour les opérations de persistance et de validation spécifiques aux utilisateurs.
    // Simule les fonctionnalités de base d'un gestionnaire d'identité (comme IdentityManager dans ASP.NET Identity).
    public interface IUserRepository
    {
        // CRUD de base
        Task<User?> GetUserByIdAsync(int id);
        
        // Opérations d'authentification et de vérification
        Task<bool> IsUserExistAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);

        // Opérations de création et de mise à jour
        Task<User> CreateUserAsync(User user, string password);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUserPasswordAsync(User user, string newPassword);
    }
}
