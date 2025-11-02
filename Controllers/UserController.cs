// -------------------------
// IMPORTS ET DÉPENDANCES
// -------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Services;
using ASPPorcelette.API.DTOs.User;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using ASPPorcelette.API.DTOs.Adherent;

namespace ASPPorcelette.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService; // ✅ Renommez ISenseiService en IUserService

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService) // ✅ Changez ici
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService; // ✅ Et ici
        }

        // -------------------------
        // GESTION DU PROFIL UTILISATEUR
        // -------------------------
        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei,Adherent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyProfile()
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
                user.Telephone,
                user.RueEtNumero,
                user.Ville,
                user.CodePostal,
                user.Grade,
                user.PhotoUrl,
                user.Bio,
                user.Statut,
                user.DateNaissance,
                user.DateAdhesion,
                user.DateRenouvellement,
                user.DisciplineId, // ✅ Pour les Sensei
                Roles = roles
            });
        }

        [HttpPut("{userId}/profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromForm] UserUpdateDto updateDto)
        {
            // Vérifie si l’utilisateur existe
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "Utilisateur introuvable." });

            // Appelle ton service avec l’ID du user ciblé
            var result = await _userService.UpdateUserProfileAsync(userId, updateDto);

            if (result.Succeeded)
                return Ok(new { Message = "Profil de l’utilisateur mis à jour avec succès." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors, Message = "Échec de la mise à jour du profil." });
        }


        [HttpPut("admin/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserByAdmin([FromRoute] Guid id, [FromForm] UserAdminUpdateDto updateDto)
        {
            if (string.IsNullOrEmpty(updateDto.UserId) || !Guid.TryParse(updateDto.UserId, out var dtoGuid) || id != dtoGuid)
            {
                return BadRequest(new { Message = "L'ID dans la route ne correspond pas à l'ID utilisateur dans les données ou l'ID est invalide." });
            }

            // 🚨 CORRECTION ICI : Appeler la nouvelle méthode de service
            var result = await _userService.UpdateUserByAdminAsync(id.ToString(), updateDto); // 👈 Utilisez UpdateUserByAdminAsync

            if (result.Succeeded)
            {
                return Ok(new { Message = "Utilisateur mis à jour par l'Administrateur avec succès." });
            }

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors, Message = "Échec de la mise à jour de l'utilisateur." });
        }
        // -------------------------
        // GESTION DES INSCRIPTIONS
        // -------------------------

        [HttpPost("register/sensei")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterSensei([FromForm] UserCreationDto registrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUserWithProfileAsync(registrationDto, "Sensei");

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Inscription Sensei réussie." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors, Message = "Échec de l'inscription Sensei." });
        }

        [HttpPost("register/adherent")]
        public async Task<IActionResult> CreateAdherent([FromBody] AdherentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // On crée un nouvel utilisateur
            var newUser = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Telephone = dto.Telephone,
                RueEtNumero = dto.Adresse,
                Ville = dto.Ville ?? "N/A",          // Utiliser le DTO si renseigné
                CodePostal = dto.CodePostal ?? "00000", // idem
                Statut = 1, // actif par défaut
                DateNaissance = dto.DateDeNaissance,
                DateAdhesion = dto.DateAdhesion,
                DateRenouvellement = dto.DateRenouvellement,
                DateCreation = DateTime.Now,
                Bio = "",   // valeur par défaut
                Grade = "", // valeur par défaut
                PhotoUrl = "",
                DisciplineId = dto.DisciplineId    // affectation si fournie
            };

            // Créer l'utilisateur sans mot de passe
            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Ajouter le rôle Adherent
            await _userManager.AddToRoleAsync(newUser, "Adherent");

            return Ok(new { message = "Adhérent créé avec succès", userId = newUser.Id });
        }



        // -------------------------
        // ADMINISTRATION
        // -------------------------

        [HttpGet("admin/list")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
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
                    RueEtNumero = user.RueEtNumero,
                    DateNaissance = user.DateNaissance,
                    user.Ville,
                    user.CodePostal,
                    user.Telephone,
                    user.Grade,
                    user.Bio,
                    user.Statut,
                    user.DateAdhesion,
                    user.DateCreation,
                    user.DateRenouvellement,
                    user.DisciplineId, // Pour les Sensei
                    Roles = roles.ToList()
                });
            }

            return Ok(userList);
        }

        [HttpGet("admin/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
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
                user.Telephone,
                user.RueEtNumero,
                user.Ville,
                user.CodePostal,
                user.Grade,
                user.PhotoUrl,
                user.Bio,
                user.Statut,
                user.DateNaissance,
                user.DateAdhesion,
                user.DateRenouvellement,
                user.DisciplineId,
                Roles = roles
            });
        }

        [HttpPost("admin/create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromForm] UserCreationDto createDto, [FromQuery] string role = "Adherent")
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUserWithProfileAsync(createDto, role);

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Utilisateur créé avec succès." });

            var errors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(new { Errors = errors });
        }

        [HttpDelete("admin/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {userId} non trouvé." });

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user.Id == currentUserId)
                return BadRequest(new { Message = "Vous ne pouvez pas supprimer votre propre compte." });

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return NoContent();

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // -------------------------
        // GESTION DES RÔLES
        // -------------------------

        [HttpGet("admin/roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        [HttpPost("admin/roles/assign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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

        [HttpPost("admin/roles/remove")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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

        public class AssignRoleDto
        {
            public string UserId { get; set; } = string.Empty;
            public string RoleName { get; set; } = string.Empty;
        }
    }
}