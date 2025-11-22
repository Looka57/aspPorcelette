using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// Imports des DTOs et Services
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Models.Identity.Dto;
using ASPPorcelette.API.Services.Identity;
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.User;
using Microsoft.AspNetCore.RateLimiting; // Pour UpdateRoleRequestDto

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("LoginRateLimit")] // Applique la limite à tout le contrôleur
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(
            IAuthService authService, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // =====================================================================
        // INSCRIPTION (Public)
        // =====================================================================

        /// <summary>
        /// Inscrit un nouvel utilisateur avec le rôle "Student" par défaut
        /// POST: api/Auth/Register
        /// </summary>
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDto
                {
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray(),
                    IsSuccess = false
                });
            }

            // Utilisation du service d'authentification pour l'inscription
            var result = await _authService.RegisterAsync(model, "Student");

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // =====================================================================
        // CONNEXION (Public)
        // =====================================================================

        /// <summary>
        /// Connecte un utilisateur et retourne un token JWT
        /// POST: api/Auth/Login
        /// </summary>
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDto
                {
                    Errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToArray(),
                    IsSuccess = false
                });
            }

            var result = await _authService.LoginAsync(model);

            if (!result.IsSuccess)
            {
                return Unauthorized(result);
            }

            // OPTIONNEL : Restriction d'accès par rôle
            // Si vous voulez que seuls les Sensei/Admin puissent se connecter via cette route,
            // décommentez le code ci-dessous :
            
            /*
            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();

            if (!roles.Contains("Sensei") && !roles.Contains("Admin"))
            {
                return Unauthorized(new AuthResultDto
                {
                    Errors = new[] { "Accès refusé. Seuls les Sensei et Admins peuvent se connecter." },
                    IsSuccess = false
                });
            }
            */

            return Ok(result);
        }

        // =====================================================================
        // GESTION DES RÔLES (Admin uniquement)
        // =====================================================================

        /// <summary>
        /// Met à jour le rôle d'un utilisateur
        /// POST: api/Auth/UpdateRole
        /// </summary>
        [HttpPost("UpdateRole")]
        [Authorize(Roles = "Admin")] // Sécurisé pour les Admins uniquement
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Liste des rôles autorisés
            var allowedRoles = new List<string> { "Admin", "Sensei", "Student" };
            
            if (!allowedRoles.Contains(request.NewRole, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new 
                { 
                    Message = $"Le rôle '{request.NewRole}' n'est pas valide. Rôles autorisés : {string.Join(", ", allowedRoles)}." 
                });
            }

            var success = await _authService.UpdateUserRoleAsync(request.UserId, request.NewRole);

            if (success)
            {
                return Ok(new 
                { 
                    Message = $"Rôle de l'utilisateur {request.UserId} mis à jour vers '{request.NewRole}' avec succès." 
                });
            }

            return NotFound(new 
            { 
                Message = $"Utilisateur avec ID {request.UserId} non trouvé." 
            });
        }

        // =====================================================================
        // ENDPOINTS SUPPLÉMENTAIRES (Optionnels)
        // =====================================================================

        /// <summary>
        /// Vérifie si un email est déjà utilisé
        /// GET: api/Auth/CheckEmail?email=test@example.com
        /// </summary>
        [HttpGet("CheckEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckEmailAvailability([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { Message = "L'email est requis." });
            }

            var userExists = await _userManager.FindByEmailAsync(email);
            
            return Ok(new 
            { 
                IsAvailable = userExists == null,
                Message = userExists == null 
                    ? "Cet email est disponible." 
                    : "Cet email est déjà utilisé."
            });
        }

        /// <summary>
        /// Récupère les rôles disponibles dans le système
        /// GET: api/Auth/Roles
        /// </summary>
        [HttpGet("Roles")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAvailableRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }
    }
}