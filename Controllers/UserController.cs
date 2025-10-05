using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Services;
using ASPPorcelette.API.DTOs.User;
using ASPPorcelette.DTOs;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ASPPorcelette.API.Controllers
{
    /// <summary>
    /// Gestion complète des utilisateurs (profil personnel + administration)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISenseiService _senseiService;

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ISenseiService senseiService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _senseiService = senseiService;
        }

        // =========================================================================
        // SECTION 1 : GESTION DU PROFIL PERSONNEL (Utilisateur connecté)
        // =========================================================================

        /// <summary>
        /// Récupère les informations du profil de l'utilisateur connecté
        /// GET: /api/User/profile
        /// </summary>
        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyProfile() // Nom cohérent avec l'action
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
                return Unauthorized(new { Message = "Impossible de trouver l'identifiant utilisateur." });

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return NotFound(new { Message = "Utilisateur non trouvé." });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName,
                user.Nom,
                user.Prenom,
                Roles = roles
            });
        }

        /// <summary>
        /// Met à jour le profil de l'utilisateur connecté
        /// PUT: /api/User/profile
        /// </summary>
        [HttpPut("profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserUpdateDto updateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
                return Unauthorized(new { Message = "Impossible de trouver l'identifiant utilisateur." });

            var result = await _senseiService.UpdateUserProfileAsync(userId, updateDto);

            if (result.Succeeded)
                return Ok(new { Message = "Profil mis à jour avec succès." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors, Message = "Échec de la mise à jour du profil." });
        }

        // =========================================================================
        // SECTION 2 : INSCRIPTION (PUBLIC - Sans authentification)
        // =========================================================================

        /// <summary>
        /// Inscription d'un nouveau Sensei (publique)
        /// POST: /api/User/register/sensei
        /// </summary>
        [HttpPost("register/sensei")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterSensei([FromBody] UserCreationDto registrationDto)
        {
            // Forcer IsSensei à true
            registrationDto.IsSensei = true;
            
            var result = await _senseiService.CreateUserWithProfileAsync(registrationDto);

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Inscription Sensei réussie." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors, Message = "Échec de l'inscription Sensei." });
        }

        /// <summary>
        /// Inscription d'un nouvel Adhérent (publique)
        /// POST: /api/User/register/adherent
        /// </summary>
        [HttpPost("register/adherent")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAdherent([FromBody] UserCreationDto registrationDto)
        {
            // Forcer IsSensei à false pour un adhérent
            registrationDto.IsSensei = false;
            
            var result = await _senseiService.CreateUserWithProfileAsync(registrationDto);

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Inscription Adhérent réussie." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors, Message = "Échec de l'inscription Adhérent." });
        }

        // =========================================================================
        // SECTION 3 : ADMINISTRATION (Réservé aux Admins et Senseis)
        // =========================================================================

        /// <summary>
        /// Liste tous les utilisateurs du système
        /// GET: /api/User/admin/list
        /// </summary>
        [HttpGet("/api/User/admin/list")] 
        [Authorize(Roles = "Admin,Sensei")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Roles = roles.ToList()
                });
            }

            return Ok(userList);
        }

        /// <summary>
        /// Récupère un utilisateur spécifique par son ID
        /// GET: /api/User/admin/{userId}
        /// </summary>
        [HttpGet("admin/{userId}")]
        [Authorize(Roles = "Admin,Sensei")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {userId} non trouvé." });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName,
                user.Nom,
                user.Prenom,
                Roles = roles
            });
        }

        /// <summary>
        /// Crée un nouvel utilisateur (Admin/Sensei uniquement)
        /// POST: /api/User/admin/create
        /// </summary>
        [HttpPost("admin/create")]
        [Authorize(Roles = "Admin,Sensei")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreationDto createDto)
        {
            var result = await _senseiService.CreateUserWithProfileAsync(createDto);

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Utilisateur créé avec succès." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }

        /// <summary>
        /// Supprime un utilisateur (Admin uniquement)
        /// DELETE: /api/User/admin/{userId}
        /// </summary>
        [HttpDelete("admin/{userId}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {userId} non trouvé." });

            // Sécurité : Empêcher la suppression de son propre compte
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user.Id == currentUserId)
                return BadRequest(new { Message = "Vous ne pouvez pas supprimer votre propre compte." });

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return NoContent();

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // =========================================================================
        // SECTION 4 : GESTION DES RÔLES (Admin uniquement)
        // =========================================================================

        /// <summary>
        /// Liste tous les rôles disponibles
        /// GET: /api/User/admin/roles
        /// </summary>
        [HttpGet("admin/roles")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        /// <summary>
        /// Attribue un rôle à un utilisateur
        /// POST: /api/User/admin/roles/assign
        /// </summary>
        [HttpPost("admin/roles/assign")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleName))
                return BadRequest(new { Message = "L'identifiant utilisateur et le nom du rôle sont requis." });

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {model.UserId} non trouvé." });

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
                return NotFound(new { Message = $"Le rôle '{model.RoleName}' n'existe pas." });

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
                return Ok(new { Message = $"Le rôle '{model.RoleName}' a été attribué à l'utilisateur." });

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        /// <summary>
        /// Retire un rôle d'un utilisateur
        /// POST: /api/User/admin/roles/remove
        /// </summary>
        [HttpPost("admin/roles/remove")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleName))
                return BadRequest(new { Message = "L'identifiant utilisateur et le nom du rôle sont requis." });

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {model.UserId} non trouvé." });

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (result.Succeeded)
                return Ok(new { Message = $"Le rôle '{model.RoleName}' a été retiré à l'utilisateur." });

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // =========================================================================
        // SECTION 5 : ENDPOINTS UTILITAIRES
        // =========================================================================

        /// <summary>
        /// Test simple pour vérifier que le contrôleur fonctionne
        /// GET: /api/User/test
        /// </summary>
        [HttpGet("test")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Test()
        {
            return Ok(new 
            { 
                Message = "UserController fonctionne correctement !",
                DateTime = DateTime.UtcNow 
            });
        }
    }

    // DTO pour l'attribution de rôle
    public class AssignRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}