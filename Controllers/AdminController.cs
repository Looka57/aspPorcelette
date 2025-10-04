// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using ASPPorcelette.API.Models.Identity;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Collections.Generic;

// // Seuls les utilisateurs avec le rôle "Admin" peuvent accéder à ce contrôleur.
// [Authorize(Roles = "Admin")]
// [ApiController]
// [Route("api/[controller]")] // Route de base: /api/Admin
// public class AdminController : ControllerBase
// {
//     private readonly UserManager<User> _userManager;
//     private readonly RoleManager<IdentityRole> _roleManager;

//     public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
//     {
//         _userManager = userManager;
//         _roleManager = roleManager;
//     }

//     // =========================================================================
//     // GESTION DES UTILISATEURS
//     // =========================================================================

//     /// <summary>
//     /// Récupère la liste complète des utilisateurs du système.
//     /// Chemin: GET /api/Admin/users
//     /// </summary>
//     [HttpGet("users")]
//     public ActionResult<IEnumerable<User>> GetUsersList()
//     {
//         // On récupère tous les utilisateurs Identity
//         var users = _userManager.Users.ToList();
//         return Ok(users);
//     }
    
//     /// <summary>
//     /// Supprime un utilisateur par son ID.
//     /// Chemin: DELETE /api/Admin/user/{userId}
//     /// </summary>
//     [HttpDelete("user/{userId}")]
//     public async Task<IActionResult> DeleteUser(string userId)
//     {
//         var user = await _userManager.FindByIdAsync(userId);
        
//         if (user == null)
//         {
//             return NotFound($"Utilisateur avec ID {userId} non trouvé.");
//         }

//         var result = await _userManager.DeleteAsync(user);

//         if (result.Succeeded)
//         {
//             return Ok(new { Message = $"Utilisateur {userId} supprimé avec succès." });
//         }

//         return BadRequest(new { Errors = result.Errors, Message = "Échec de la suppression de l'utilisateur." });
//     }

//     // =========================================================================
//     // GESTION DES RÔLES
//     // =========================================================================
    
//     /// <summary>
//     /// Récupère la liste de tous les rôles disponibles.
//     /// Chemin: GET /api/Admin/roles
//     /// </summary>
//     [HttpGet("roles")]
//     public ActionResult<IEnumerable<string>> GetRoles()
//     {
//         var roles = _roleManager.Roles.Select(r => r.Name).ToList();
//         return Ok(roles);
//     }
    
//     // DTO pour l'attribution de rôle
//     public class AssignRoleModel
//     {
//         public string UserId { get; set; }
//         public string RoleName { get; set; }
//     }

//     /// <summary>
//     /// Attribue un rôle à un utilisateur spécifique.
//     /// Chemin: POST /api/Admin/assign-role
//     /// </summary>
//     [HttpPost("assign-role")]
//     public async Task<IActionResult> AssignRole([FromBody] AssignRoleModel model)
//     {
//         if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleName))
//         {
//             return BadRequest("L'identifiant utilisateur et le nom du rôle sont requis.");
//         }

//         var user = await _userManager.FindByIdAsync(model.UserId);
//         if (user == null) return NotFound($"Utilisateur avec ID {model.UserId} non trouvé.");

//         var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
//         if (!roleExists) return NotFound($"Le rôle '{model.RoleName}' n'existe pas.");

//         // Ajouter le rôle
//         var result = await _userManager.AddToRoleAsync(user, model.RoleName);

//         if (result.Succeeded)
//         {
//             return Ok(new { Message = $"Le rôle '{model.RoleName}' a été attribué à l'utilisateur." });
//         }

//         return BadRequest(new { Errors = result.Errors, Message = "Échec de l'attribution du rôle." });
//     }
// }
