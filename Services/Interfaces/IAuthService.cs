
// >>> AJOUT CRITIQUE : Le using pour le AuthResultDto que vous utilisez dans le contrôleur <<<
using ASPPorcelette.API.Models.Identity.Dto; 
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Identity
{
    /// <summary>
    /// Définit le contrat pour les opérations d'enregistrement et de connexion.
    /// </summary>
    public interface IAuthService
    {
        // 1. CHANGEMENT DE TYPE : Le type de retour doit être AuthResultDto
        Task<AuthResultDto?> RegisterAsync(RegisterRequestDto request, string role);

        // 2. CHANGEMENT DE TYPE : Le type de retour doit être AuthResultDto
        Task<AuthResultDto?> LoginAsync(LoginDto request);

                Task<bool> UpdateUserRoleAsync(string userId, string newRole);

    }
}
