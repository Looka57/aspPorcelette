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
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.DTOs; // Contient probablement UserCreationDto

[ApiController]
[Route("api/[controller]")] // Route de base: /api/User
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ISenseiService _senseiService;
    // Si vous aviez un IAdherentService, il faudrait l'injecter ici aussi.

    public UserController(
    UserManager<User> userManager,
    ISenseiService senseiService)
    {
        _userManager = userManager;
        _senseiService = senseiService;
    }

    // =========================================================================
    // CHEMINS D'INSCRIPTION / AUTHENTIFICATION (PUBLIC)
    // =========================================================================

    /// <summary>
        /// Inscription Sensei. Chemin: POST /api/User/register/sensei
        /// </summary>
    [HttpPost("register/sensei")]
    [AllowAnonymous]
    // CORRECTION : Nous changeons SenseiDto en UserCreationDto pour correspondre à l'interface
    public async Task<IActionResult> RegisterSenseiAsync([FromBody] UserCreationDto registrationDto)
    {
        // La logique de création et d'attribution de rôle est déléguée au service
        // CORRECTION : Nous utilisons la variable qui existe : registrationDto
        var result = await _senseiService.CreateUserWithProfileAsync(registrationDto);

        if (result.Succeeded)
        {
            return StatusCode(201, new { Message = "Inscription Sensei réussie. Utilisateur créé." });
        }

        var errors = result.Errors.Select(e => e.Description).ToList();
        return BadRequest(new { Errors = errors, Message = "Échec de l'inscription Sensei." });
    }

    // Ajoutez ici la route de Login, par exemple : [HttpPost("login")]...

    // =========================================================================
    // CHEMINS D'ACCÈS AUTHENTIFIÉ (MON PROFIL)
    // =========================================================================

    /// <summary>
        /// Qui suis-je ? Chemin: GET /api/User/whoami
        /// </summary>
    [HttpGet("whoami")]
    [Authorize]
    public async Task<IActionResult> WhoAmI()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized("Impossible de trouver l'identifiant utilisateur.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Utilisateur non trouvé.");

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
        /// Mise à jour de mon profil. Chemin: PUT /api/User/profile
        /// </summary>
    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserUpdateDto updateDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized(new { Message = "Utilisateur non identifié." });

        var result = await _senseiService.UpdateUserProfileAsync(userIdClaim.Value, updateDto);

        if (result.Succeeded) return Ok(new { Message = "Profil mis à jour avec succès." });

        var errors = result.Errors.Select(e => e.Description).ToList();
        return BadRequest(new { Errors = errors, Message = "Échec de la mise à jour du profil." });
    }

    /// <summary>
        /// Exemple: Récupère les données spécifiques au profil Sensei de l'utilisateur connecté.
        /// Chemin: GET /api/User/sensei-data
        /// </summary>
    [HttpGet("sensei-data")]
    [Authorize(Roles = "Sensei")] // Seulement si l'utilisateur est Sensei
    public IActionResult GetSenseiData()
    {
        return Ok(new { Message = "Données Sensei spécifiques (à implémenter via SenseiService)." });
    }
}
