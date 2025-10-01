using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Models.Identity.Dto; // Doit contenir AuthResultDto
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// Nous avons besoin de cette classe DTO pour le corps de la requête du service.
using RegisterRequestDto = ASPPorcelette.API.Models.DTOs.RegisterRequestDto; 
using AuthResultDto = ASPPorcelette.API.Models.Identity.Dto.AuthResultDto;
using ASPPorcelette.API.Services.Identity;

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

        public SenseiController(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        // GET: api/Sensei/ListUsers
        /// <summary>
        /// Liste tous les utilisateurs et leurs rôles (Sensei et Students).
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

        // POST: api/Sensei/CreateUser
        /// <summary>
        /// Crée un nouvel utilisateur (Student ou Sensei) avec le rôle spécifié.
        /// Nécessite le rôle 'Sensei'.
        /// </summary>
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterWithRoleDto model)
        {
            // Vérifie que le rôle est valide
            if (model.Role != "Student" && model.Role != "Sensei")
            {
                // Utilisation de AuthResultDto
                return BadRequest(new AuthResultDto { Errors = new[] { "Rôle non valide. Doit être 'Student' ou 'Sensei'." }, IsSuccess = false });
            }

            // Le service d'authentification doit renvoyer un AuthResultDto
            var result = await _authService.RegisterAsync(
                new RegisterRequestDto 
                {
                    Email = model.Email,
                    Password = model.Password
                }, 
                model.Role 
            ); 

            // Utilisation de AuthResultDto pour la vérification et le retour
            if (result.IsSuccess)
            {
                return Ok(new AuthResultDto { IsSuccess = true, UserId = result.UserId });
            }

            // Ici, nous accédons à result.Errors et result.IsSuccess, ce qui nécessite AuthResultDto.
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
            
            // Sécurité : Interdire au Sensei de se supprimer lui-même (facultatif mais recommandé)
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
