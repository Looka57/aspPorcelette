using ASPPorcelette.API.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

// Importation des DTOs spécifiques
using RegisterRequestDto = ASPPorcelette.API.Models.DTOs.RegisterRequestDto; 
using AuthResultDto = ASPPorcelette.API.Models.Identity.Dto.AuthResultDto;

using ASPPorcelette.API.Services.Identity;
using ASPPorcelette.API.Services; 
using Microsoft.AspNetCore.Http;
using ASPPorcelette.API.Models.Identity.Dto;
using ASPPorcelette.DTOs; // Nécessaire pour StatusCodes

namespace ASPPorcelette.API.Controllers
{
    // Sécurise l'intégralité du contrôleur. Seuls les utilisateurs avec le rôle "Sensei" peuvent y accéder.
    [Authorize(Roles = "Sensei")] 
    [Route("api/[controller]")]
    [ApiController]
    public class SenseiController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService; 
        private readonly ISenseiService _senseiService; // Injecté pour l'orchestration métier

        // Constructeur mis à jour pour injecter ISenseiService
        public SenseiController(
            UserManager<User> userManager, 
            IAuthService authService,
            ISenseiService senseiService)
        {
            _userManager = userManager;
            _authService = authService;
            _senseiService = senseiService;
        }


        // POST: api/Sensei/CreateUserWithProfile
        /// <summary>
        /// Crée un nouvel utilisateur et lui associe immédiatement un profil métier (Adherent ou Sensei). 
        /// Utilise le service d'orchestration ISenseiService.
        /// </summary>
        [HttpPost("CreateUserWithProfile")] // Nom de route clarifié
        public async Task<IActionResult> CreateUserWithProfile([FromBody] UserCreationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // Appel au service d'orchestration (utilise l'instance injectée)
            var result = await _senseiService.CreateUserWithProfileAsync(model);
            
            if (result.Succeeded)
            {
                return Ok(new { Message = "Utilisateur et profil créés avec succès." });
            }
            else
            {
                return BadRequest(new { Errors = result.Errors });
            }
        }
        
        // GET: api/Sensei/ListUsers
        /// <summary>
        /// Liste tous les utilisateurs et leurs rôles.
        /// </summary>
        [HttpGet("ListUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserInfoDto>))]
        public async Task<IActionResult> ListUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<UserInfoDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); 
                
                userList.Add(new UserInfoDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList() 
                });
            }

            return Ok(userList);
        }

        // POST: api/Sensei/RegisterUserWithRole
        /// <summary>
        /// Crée un nouvel utilisateur Identity et lui assigne un rôle (sans créer de profil métier associé).
        /// Utilise le service d'authentification IAuthService.
        /// </summary>
        [HttpPost("RegisterUserWithRole")] // Nom de route clarifié
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUserWithRole([FromBody] RegisterWithRoleDto model)
        {
            // Vérifie que le rôle est valide
            if (model.Role != "Adherent" && model.Role != "Sensei")
            {
                return BadRequest(new AuthResultDto { Errors = new[] { "Rôle non valide. Doit être 'Adherent' ou 'Sensei'." }, IsSuccess = false });
            }

            var result = await _authService.RegisterAsync(
                new RegisterRequestDto 
                {
                    Email = model.Email,
                    Password = model.Password
                }, 
                model.Role 
            ); 

            if (result.IsSuccess)
            {
                return Ok(new AuthResultDto { IsSuccess = true, UserId = result.UserId });
            }

            return BadRequest(new AuthResultDto { Errors = result.Errors, IsSuccess = false });
        }

        // DELETE: api/Sensei/DeleteUser/{userId}
        /// <summary>
        /// Supprime un utilisateur par son ID.
        /// </summary>
        [HttpDelete("DeleteUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Utilisateur avec l'ID {userId} non trouvé.");
            }
            
            // Sécurité : Interdire au Sensei de se supprimer lui-même
            if (user.Id == User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value)
            {
                return BadRequest("Vous ne pouvez pas supprimer votre propre compte Sensei.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return NoContent(); // 204 No Content pour une suppression réussie
            }

            // Retourne les erreurs d'Identity si la suppression échoue
            return BadRequest(result.Errors.Select(e => e.Description));
        }
    }
}
