using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Models.Identity.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Inscription d'un nouvel utilisateur avec attribution de rôle
        /// </summary>
        public async Task<AuthResultDto> RegisterAsync(RegisterRequestDto request, string role)
        {
            // 1. Vérifier si l'utilisateur existe déjà
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new[] { "L'utilisateur avec cet e-mail existe déjà." }
                };
            }

            // 2. Créer l'objet utilisateur
            var newUser = new User
            {
                UserName = request.UserName ?? request.Email,
                Email = request.Email,
                Nom = request.Nom,
                Prenom = request.Prenom,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // 3. Créer l'utilisateur dans la base de données
            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            // 4. Attribuer le rôle spécifié
            var roleResult = await _userManager.AddToRoleAsync(newUser, role);
            if (!roleResult.Succeeded)
            {
                // Supprimer l'utilisateur si l'attribution du rôle échoue
                await _userManager.DeleteAsync(newUser);
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = roleResult.Errors.Select(e => e.Description).ToArray()
                };
            }

            // 5. Générer le token JWT
            var token = await _tokenService.CreateTokenAsync(newUser);

            // 6. Récupérer les rôles pour la réponse
            var roles = await _userManager.GetRolesAsync(newUser);

            // 7. Retourner la réponse de succès
            return new AuthResultDto
            {
                IsSuccess = true,
                UserId = newUser.Id,
                Email = newUser.Email!,
                Token = token,
                Roles = roles.ToList(),
                Errors = Array.Empty<string>()
            };
        }

        /// <summary>
        /// Connexion d'un utilisateur existant
        /// </summary>
        public async Task<AuthResultDto> LoginAsync(LoginDto request)
        {
            // 1. Trouver l'utilisateur par Email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new[] { "Identifiants invalides." }
                };
            }

            // 2. Vérifier le mot de passe
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return new AuthResultDto
                {
                    IsSuccess = false,
                    Errors = new[] { "Identifiants invalides." }
                };
            }

            // 3. Générer le token JWT
            var token = await _tokenService.CreateTokenAsync(user);

            // 4. Récupérer les rôles de l'utilisateur
            var roles = await _userManager.GetRolesAsync(user);

            // 5. Retourner la réponse de succès
            return new AuthResultDto
            {
                IsSuccess = true,
                UserId = user.Id,
                Email = user.Email!,
                Token = token,
                Roles = roles.ToList(),
                Errors = Array.Empty<string>()
            };
        }

        /// <summary>
        /// Met à jour le rôle d'un utilisateur (remplace tous les rôles existants)
        /// </summary>
        public async Task<bool> UpdateUserRoleAsync(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            // 1. Récupérer les rôles actuels
            var currentRoles = await _userManager.GetRolesAsync(user);

            // 2. Supprimer tous les rôles actuels
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return false;
                }
            }

            // 3. Ajouter le nouveau rôle
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            
            return addResult.Succeeded;
        }
    }
}