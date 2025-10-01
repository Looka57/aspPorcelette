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

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ISenseiService _senseiService; // Nouveau : Injection du service

    public UserController(
        UserManager<User> userManager, 
        RoleManager<IdentityRole> roleManager,
        ISenseiService senseiService) // Nouveau : Injection dans le constructeur
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _senseiService = senseiService;
    }

    // =========================================================================
    // CHEMINS D'ACCÈS PUBLIC / AUTHENTIFIÉ
    // =========================================================================

    /// <summary>
    /// Retourne l'identité de l'utilisateur actuellement connecté (Who Am I).
    /// Chemin: /api/User/whoami
    /// </summary>
    [HttpGet("whoami")]
    [Authorize] // Nécessite une authentification simple
    public async Task<IActionResult> WhoAmI()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("Impossible de trouver l'identifiant utilisateur.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("Utilisateur non trouvé.");
        }

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
    /// Permet à l'utilisateur connecté de mettre à jour son propre profil.
    /// Chemin: /api/User/profile
    /// </summary>
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateDto updateDto)
    {
        // Récupérer l'ID de l'utilisateur connecté
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized(new { Message = "Utilisateur non identifié." });
        }
        
        var userId = userIdClaim.Value;

        // Appeler le service pour mettre à jour l'utilisateur Identity et le profil métier (Sensei ou Adherent)
        var result = await _senseiService.UpdateUserProfileAsync(userId, updateDto);

        if (result.Succeeded)
        {
            return Ok(new { Message = "Profil mis à jour avec succès." });
        }

        // Gérer les erreurs Identity
        var errors = result.Errors.Select(e => e.Description).ToList();
        return BadRequest(new { Errors = errors, Message = "Échec de la mise à jour du profil." });
    }

    // =========================================================================
    // CHEMINS D'ADMINISTRATION (NÉCESSITENT LE RÔLE ADMIN)
    // =========================================================================

    /// <summary>
    /// Récupère la liste complète des utilisateurs (équivalent à testing-list / users).
    /// Chemin: /api/User/list
    /// </summary>
    [HttpGet("list")]
    [Authorize(Roles = "Admin")] // Rôle Admin Requis
    public ActionResult<IEnumerable<User>> GetUsersList()
    {
        // Dans une application réelle, ceci est généralement un appel EF Core:
        // var users = await _userManager.Users.ToListAsync();
        
        // Pour la démo, on simule:
        var users = _userManager.Users.ToList();
        
        return Ok(users);
    }
    
    /// <summary>
    /// Récupère la liste de tous les rôles disponibles dans l'application.
    /// Chemin: /api/User/roles
    /// </summary>
    [HttpGet("roles")]
    [Authorize(Roles = "Admin")] // Rôle Admin Requis
    public ActionResult<IEnumerable<string>> GetRoles()
    {
        var roles = _roleManager.Roles.Select(r => r.Name).ToList();
        return Ok(roles);
    }
    
    /// <summary>
    /// Modèle pour l'attribution de rôle
    /// </summary>
    public class AssignRoleModel
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }

    /// <summary>
    /// Attribue un rôle à un utilisateur spécifique.
    /// Chemin: /api/User/assign-role
    /// </summary>
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")] // Rôle Admin Requis
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
    {
        if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleName))
        {
            return BadRequest("L'identifiant utilisateur et le nom du rôle sont requis.");
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return NotFound($"Utilisateur avec ID {model.UserId} non trouvé.");
        }

        var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
        if (!roleExists)
        {
            return NotFound($"Le rôle '{model.RoleName}' n'existe pas.");
        }

        // Ajouter le rôle
        var result = await _userManager.AddToRoleAsync(user, model.RoleName);

        if (result.Succeeded)
        {
            return Ok(new { Message = $"Le rôle '{model.RoleName}' a été attribué à l'utilisateur." });
        }

        return BadRequest(new { Errors = result.Errors, Message = "Échec de l'attribution du rôle." });
    }
}
