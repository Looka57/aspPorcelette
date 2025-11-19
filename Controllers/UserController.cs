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
using ASPPorcelette.API.DTOs.Adherent;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ASPPorcelette.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // -------------------------
        // 🔹 Dépendances injectées
        // -------------------------
        private readonly UserManager<User> _userManager;      // Gestion des utilisateurs ASP.NET Identity
        private readonly RoleManager<IdentityRole> _roleManager; // Gestion des rôles
        private readonly IUserService _userService;           // Service métier pour la logique utilisateur

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
        }

        // ================================================================
        // 🧩 SECTION 1 : GESTION DU PROFIL UTILISATEUR
        // ================================================================

        /// <summary>
        /// 🔹 Récupère les informations du profil de l'utilisateur connecté.
        /// Accessible par tous les rôles (Admin, Sensei, Adhérent).
        /// </summary>
        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei,Adherent")]
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
                user.DisciplineId,
                Roles = roles
            });
        }

        /// <summary>
        /// 🔹 Mise à jour du profil utilisateur.
        /// Accessible uniquement à l’utilisateur concerné ou à un administrateur.
        /// </summary>
        [HttpPut("{userId}/profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei,Adherent")]
        public async Task<IActionResult> UpdateUserProfile(string userId, [FromBody] UserUpdateDto updateDto)
        {
            // ✅ Vérifie si l'utilisateur courant est soit admin, soit propriétaire du profil
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            bool isOwner = currentUserId == userId;

            if (!isAdmin && !isOwner)
                return Forbid();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = "Utilisateur introuvable." });

            // ✅ Appel au service métier pour faire la mise à jour
            var result = await _userService.UpdateUserProfileAsync(userId, updateDto);

            if (result.Succeeded)
                return Ok(new { Message = "Profil mis à jour avec succès." });

            return BadRequest(new
            {
                Errors = result.Errors.Select(e => e.Description),
                Message = "Échec de la mise à jour du profil."
            });
        }

        // ================================================================
        // 🧩 SECTION 2 : GESTION ADMIN / SENSEI (Modification complète)
        // ================================================================

        /// <summary>
        /// 🔹 Mise à jour d’un utilisateur par un administrateur ou un sensei.
        /// Utilisé dans le back-office.
        /// </summary>
        /// <summary>
        /// 🔹 Mise à jour d’un utilisateur par un administrateur ou un sensei.
        /// Utilisé dans le back-office.
        /// </summary>
        [HttpPut("admin/{userId}")] // 💡 Utilisation de {userId} pour plus de clarté
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Sensei")]
        // 🟢 CORRECTION: L'ID est passé comme string, et on utilise le DTO que nous avons harmonisé
        public async Task<IActionResult> UpdateUserByAdmin([FromRoute] string userId, [FromForm] UserUpdateDto updateDto)
        {
            // 💡 Simplification de la validation de l'ID (on utilise l'ID de la route)
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { Message = "L'ID utilisateur est manquant." });

            // 💡 (Optionnel mais recommandé si le DTO contient UserId) 
            // updateDto.UserId = userId; 

            // Le service doit utiliser l'ID de la route pour trouver l'utilisateur.
            // Nous appelons le service avec l'ID en string.
            var result = await _userService.UpdateUserByAdminAsync(userId, updateDto);

            if (result.Succeeded)
                return Ok(new { Message = "Utilisateur mis à jour avec succès par l'administrateur." });

            return BadRequest(new
            {
                Errors = result.Errors.Select(e => e.Description),
                Message = "Échec de la mise à jour de l'utilisateur."
            });
        }
        // ================================================================
        // 🧩 SECTION 3 : GESTION DES INSCRIPTIONS
        // ================================================================

        /// <summary>
        /// 🔹 Enregistre un nouveau Sensei (compte enseignant).
        /// </summary>
        [HttpPost("register/sensei")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterSensei([FromForm] UserCreationDto registrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUserWithProfileAsync(registrationDto, "Sensei");

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Inscription Sensei réussie." });

            return BadRequest(new
            {
                Errors = result.Errors.Select(e => e.Description),
                Message = "Échec de l'inscription Sensei."
            });
        }

        /// <summary>
        /// 🔹 Crée un adhérent (utilisateur sans mot de passe).
        /// </summary>
        [HttpPost("register/adherent")]
        public async Task<IActionResult> CreateAdherent([FromBody] AdherentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                // Vérifie si l'utilisateur est actif
                bool isActif = existingUser.Statut == 1 && existingUser.DateRenouvellement.HasValue &&
                               existingUser.DateRenouvellement.Value >= DateTime.Today;

                if (isActif)
                    return BadRequest(new { Message = "Un utilisateur actif existe déjà avec cet email." });

                // Réactivation de l'utilisateur inactif
                existingUser.Statut = 1;
                existingUser.DateRenouvellement = DateTime.Today.AddYears(1);
                existingUser.Nom = dto.Nom;
                existingUser.Prenom = dto.Prenom;
                existingUser.Telephone = dto.Telephone;
                existingUser.RueEtNumero = dto.Adresse;
                existingUser.Ville = dto.Ville ?? "N/A";
                existingUser.CodePostal = dto.CodePostal ?? "00000";
                existingUser.DisciplineId = dto.DisciplineId;
                existingUser.DateAdhesion = dto.DateAdhesion;
                existingUser.DateNaissance = dto.DateDeNaissance;

                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (updateResult.Succeeded)
                {
                    return Ok(new
                    {
                        Message = "Utilisateur réactivé avec succès.",
                        userId = existingUser.Id,
                        DateRenouvellement = existingUser.DateRenouvellement.Value.ToShortDateString()
                    });
                }

                return BadRequest(new { Errors = updateResult.Errors.Select(e => e.Description) });
            }

            // Création normale si l'utilisateur n'existe pas
            var newUser = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Telephone = dto.Telephone,
                RueEtNumero = dto.Adresse,
                Ville = dto.Ville ?? "N/A",
                CodePostal = dto.CodePostal ?? "00000",
                Statut = 1,
                DateNaissance = dto.DateDeNaissance,
                DateAdhesion = dto.DateAdhesion,
                DateRenouvellement = DateTime.Today.AddYears(1),
                DateCreation = DateTime.Now,
                Bio = "",
                Grade = "",
                PhotoUrl = "",
                DisciplineId = dto.DisciplineId
            };

            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                var duplicateError = createResult.Errors.FirstOrDefault(e =>
                    e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName");

                if (duplicateError != null)
                    return BadRequest(new { Message = "Cette adresse e-mail est déjà utilisée." });

                return BadRequest(new { Errors = createResult.Errors.Select(e => e.Description) });
            }

            await _userManager.AddToRoleAsync(newUser, "Adherent");

            return Ok(new
            {
                Message = "Adhérent créé avec succès",
                userId = newUser.Id,
                DateRenouvellement = newUser.DateRenouvellement.Value.ToShortDateString()
            });
        }

// UserController.cs

// ... dans une nouvelle section publique ou à la fin

/// <summary>
/// 🔹 Récupère un utilisateur spécifique par son ID pour un affichage public (sans authentification).
/// </summary>
/// <summary>
/// 🔹 Récupère un utilisateur spécifique par son ID pour un affichage public (sans authentification).
/// </summary>
[HttpGet("{userId}")]
[AllowAnonymous]
public async Task<IActionResult> GetUserProfilePublic(string userId)
{
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
        return NotFound(new { Message = $"Utilisateur avec ID {userId} non trouvé." });

    var roles = await _userManager.GetRolesAsync(user);
    
    return Ok(new
    {
        Id = user.Id,
        user.Nom,
        user.Prenom,
        user.Telephone,
        user.RueEtNumero,
        user.Ville,
        user.CodePostal,
        user.Grade,
        user.PhotoUrl,
        user.Bio,
        user.DisciplineId,
        Roles = roles
    });
}
















        // Dans UserController.cs

        // ================================================================
        // 🧩 SECTION 4 : ADMINISTRATION GÉNÉRALE
        // ================================================================

        /// <summary>
        /// 🔹 Liste tous les utilisateurs pour l’administration.
        /// </summary>
        [HttpGet("admin/list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            // 💡 CORRECTION : Utilisation du service pour obtenir la liste, 
            // qui mappe correctement en UserDto (incluant PhotoUrl).
            var userListDtos = await _userService.GetAdminUserListAsync();

            return Ok(userListDtos);
        }


        

        /// <summary>
        /// 🔹 Récupère un utilisateur spécifique via son ID.
        /// </summary>
        [HttpGet("admin/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
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

        /// <summary>
        /// 🔹 Crée un utilisateur (Admin/Sensei).
        /// </summary>
        [HttpPost("admin/create")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
        public async Task<IActionResult> CreateUser([FromForm] UserCreationDto createDto, [FromQuery] string role = "Adherent")
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUserWithProfileAsync(createDto, role);

            if (result.Succeeded)
                return StatusCode(201, new { Message = "Utilisateur créé avec succès." });

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        /// <summary>
        /// 🔹 Supprime un utilisateur (seulement par un Admin).
        /// </summary>
        [HttpDelete("admin/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {userId} non trouvé." });

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user.Id == currentUserId)
                return BadRequest(new { Message = "Vous ne pouvez pas supprimer votre propre compte." });

            var result = await _userService.DeactivateUserAsync(userId);
            if (result.Succeeded)
                return NoContent();

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // ================================================================
        // 🧩 SECTION 5 : GESTION DES RÔLES
        // ================================================================

        /// <summary>
        /// 🔹 Liste tous les rôles disponibles.
        /// </summary>
        [HttpGet("admin/roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }

        /// <summary>
        /// 🔹 Attribue un rôle à un utilisateur.
        /// </summary>
        [HttpPost("admin/roles/assign")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleName))
                return BadRequest(new { Message = "L'identifiant utilisateur et le rôle sont requis." });

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {model.UserId} non trouvé." });

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
                return NotFound(new { Message = $"Le rôle '{model.RoleName}' n'existe pas." });

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
                return Ok(new { Message = $"Rôle '{model.RoleName}' attribué avec succès." });

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        /// <summary>
        /// 🔹 Retire un rôle à un utilisateur.
        /// </summary>
        [HttpPost("admin/roles/remove")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDto model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleName))
                return BadRequest(new { Message = "L'identifiant utilisateur et le rôle sont requis." });

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound(new { Message = $"Utilisateur avec ID {model.UserId} non trouvé." });

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
            if (result.Succeeded)
                return Ok(new { Message = $"Rôle '{model.RoleName}' retiré avec succès." });

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // ================================================================
        // 🔹 STATISTIQUES ADHERENTS & RENOUVELLEMENT
        // ================================================================

        /// <summary>
        /// 🔹 Renouvelle l'adhésion d'un adhérent en mettant à jour sa DateRenouvellement.
        /// Accessible par Admin et Sensei.
        /// </summary>
        [HttpPost("admin/renouvellement/{userId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
        public async Task<IActionResult> RenewAdhesion(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { Message = "L'ID utilisateur est requis." });

            var result = await _userService.RenewAdhesionAsync(userId);

            if (result.Succeeded)
                return Ok(new { Message = "Adhésion renouvelée avec succès." });

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }


        /// <summary>
        /// 🔹 Récupère le nombre d'adhérents actifs (Statut = 1 et DateRenouvellement >= aujourd'hui).
        /// Accessible par Admin et Sensei.
        /// </summary>
        [HttpGet("admin/statistiques/actifs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Sensei")]
        public async Task<IActionResult> GetActiveAdherentsCount()
        {
            var count = await _userService.GetActiveAdherentsCountAsync();
            return Ok(new { ActiveAdherents = count });
        }

        /// <summary>
        /// 🔹 Renouvelle l'adhésion d'un utilisateur (Adhérent) en mettant à jour la DateRenouvellement.
        /// Accessible par Admin et Sensei.
        /// </summary>

        // ================================================================
        // 🧩 SECTION 6 : TEST TECHNIQUE
        // ================================================================

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok(new
            {
                Message = "✅ UserController fonctionne correctement !",
                DateTime = DateTime.UtcNow
            });
        }

        // -------------------------
        // 🔹 DTO interne
        // -------------------------
        public class AssignRoleDto
        {
            public string UserId { get; set; } = string.Empty;
            public string RoleName { get; set; } = string.Empty;
        }



        
    }

    
}
