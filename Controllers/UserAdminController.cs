// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Security.Claims;
// using System.ComponentModel.DataAnnotations;
// // VÉRIFIEZ CES DEUX LIGNES (pour RoleConstants et User)
// using ASPPorcelette.API.Constants; 
// using ASPPorcelette.API.Models.Identity; 

// namespace ASPPorcelette.API.Controler
// {
//     // Sécurisé : Seuls les utilisateurs avec le rôle "Sensei" peuvent accéder à ce contrôleur.
//     [Authorize(Roles = RoleConstants.Sensei)]
//     [Route("api/[controller]")]
//     [ApiController]
//     public class UserAdminController : ControllerBase
//     {
//         // ATTENTION : UserManager utilise votre classe 'User'
//         private readonly UserManager<User> _userManager; 
//         private readonly RoleManager<IdentityRole> _roleManager;

//         public UserAdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
//         {
//             _userManager = userManager;
//             _roleManager = roleManager;
//         }

//         // ... Reste des méthodes du contrôleur ...
        
//         // DTO pour la modification des rôles
//         public class UserRoleUpdateDTO
//         {
//             [Required]
//             public string UserId { get; set; } // ID de l'utilisateur à modifier
            
//             [Required]
//             public string NewRole { get; set; } // Le nouveau rôle à attribuer (ex: "Student", "Sensei")
//         }

//         /// <summary>
//         /// Obtient la liste de tous les utilisateurs et leurs rôles actuels.
//         /// Nécessite le rôle Sensei.
//         /// </summary>
//         [HttpGet("users")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         public async Task<IActionResult> GetAllUsers()
//         {
//             var users = await _userManager.Users.ToListAsync();

//             var usersWithRoles = new List<object>();

//             foreach (var user in users)
//             {
//                 var roles = await _userManager.GetRolesAsync(user);
//                 usersWithRoles.Add(new
//                 {
//                     user.Id,
//                     user.Email,
//                     Roles = roles.ToList()
//                 });
//             }

//             return Ok(usersWithRoles);
//         }

//         /// <summary>
//         /// Obtient la liste de tous les rôles disponibles dans le système.
//         /// Nécessite le rôle Sensei.
//         /// </summary>
//         [HttpGet("roles")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         public IActionResult GetAllRoles()
//         {
//             var roles = _roleManager.Roles.Select(r => r.Name).ToList();
//             return Ok(roles);
//         }

//         /// <summary>
//         /// Modifie le rôle d'un utilisateur existant.
//         /// Nécessite le rôle Sensei.
//         /// </summary>
//         [HttpPost("users/assign-role")]
//         [ProducesResponseType(StatusCodes.Status200OK)]
//         [ProducesResponseType(StatusCodes.Status400BadRequest)]
//         [ProducesResponseType(StatusCodes.Status404NotFound)]
//         public async Task<IActionResult> AssignRole([FromBody] UserRoleUpdateDTO dto)
//         {
//             // 1. Vérifier si l'utilisateur existe
//             var user = await _userManager.FindByIdAsync(dto.UserId);
//             if (user == null)
//             {
//                 return NotFound($"Utilisateur avec l'ID '{dto.UserId}' non trouvé.");
//             }

//             // 2. Vérifier si le rôle existe
//             var roleExists = await _roleManager.RoleExistsAsync(dto.NewRole);
//             if (!roleExists)
//             {
//                 return BadRequest($"Le rôle '{dto.NewRole}' n'existe pas.");
//             }

//             // 3. Récupérer les rôles actuels et les supprimer
//             var currentRoles = await _userManager.GetRolesAsync(user);
//             var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            
//             if (!removeResult.Succeeded)
//             {
//                 return BadRequest($"Échec de la suppression des anciens rôles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
//             }

//             // 4. Ajouter le nouveau rôle
//             var addResult = await _userManager.AddToRoleAsync(user, dto.NewRole);

//             if (!addResult.Succeeded)
//             {
//                 return BadRequest($"Échec de l'attribution du rôle '{dto.NewRole}': {string.Join(", ", addResult.Errors.Select(e => e.Description))}");
//             }

//             return Ok(new { Message = $"Rôle de l'utilisateur '{user.Email}' mis à jour en '{dto.NewRole}'." });
//         }
//     }
// }
