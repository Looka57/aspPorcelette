using ASPPorcelette.API.Models.DTOs;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Models.Identity.Dto; // Importation nécessaire pour AuthResultDto
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq; // Nécessaire pour Select().ToArray()
using System; // Nécessaire pour Array.Empty<string>()

namespace ASPPorcelette.API.Services.Identity
{
    // Le type de retour est maintenant cohérent avec IAuthService
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService; // Assurez-vous que cette dépendance est disponible

        public AuthService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // Le type de retour correspond à l'interface
        public async Task<AuthResultDto?> RegisterAsync(RegisterRequestDto request, string role)
        {
            // 1. Vérifier si l'utilisateur existe déjà
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                // Retourne un échec au lieu de null, avec un message d'erreur
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new[] { "L'utilisateur avec cet e-mail existe déjà." }
                };
            }

            // 2. Créer l'objet utilisateur
            var newUser = new User
            {
                // Si UserName n'est pas dans le DTO de requête, utilisez l'Email
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            // 3. Créer l'utilisateur dans la base de données
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                // Retourne les erreurs Identity en cas d'échec de création
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            // 4. Attribuer le rôle spécifié
            await _userManager.AddToRoleAsync(newUser, role);

            // 5. Générer le token JWT
            var token = await _tokenService.CreateTokenAsync(newUser);

            // 6. Retourner la réponse de succès (AuthResultDto)
            return new AuthResultDto
            {
                IsSuccess = true,
                UserId = newUser.Id,
                Email = newUser.Email!,
                Token = token,
                Errors = Array.Empty<string>()
            };
        }

        // Le type de retour correspond à l'interface
        public async Task<AuthResultDto?> LoginAsync(LoginDto request)
        {
            // 1. Trouver l'utilisateur par Email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new[] { "Identifiants invalides (email ou mot de passe incorrect)." }
                };
            }

            // 2. Vérifier le mot de passe
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new[] { "Identifiants invalides (email ou mot de passe incorrect)." }
                };
            }

            // 3. Générer le token JWT
            var token = await _tokenService.CreateTokenAsync(user);

            // 4. Retourner la réponse de succès (AuthResultDto)
            return new AuthResultDto
            {
                IsSuccess = true,
                UserId = user.Id,
                Email = user.Email!,
                Token = token,
                Errors = Array.Empty<string>()
            };
        }
        
         public async Task<bool> UpdateUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false; // Utilisateur non trouvé
            }

            // 1. Récupérer les rôles actuels
            var currentRoles = await _userManager.GetRolesAsync(user);

            // 2. Supprimer tous les rôles actuels
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                // Enregistrez une erreur ou retournez false si la suppression échoue
                // (Exemple : _logger.LogError($"Échec de la suppression des rôles pour l'utilisateur {userId}: {removeResult.Errors.First().Description}");)
                return false;
            }

            // 3. Ajouter le nouveau rôle
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                // Enregistrez une erreur si l'ajout échoue
                return false;
            }

            return true; // Succès
        }
    }
}
