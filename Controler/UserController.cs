using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASPPorcelette.API.Models.Identity; 
using System.Security.Claims; 
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Nécessaire pour ToListAsync()

// Utilisez votre classe User
using User = ASPPorcelette.API.Models.Identity.User; 

namespace ASPPorcelette.API.Controllers
{
    // L'autorisation s'applique toujours ici par défaut : [Authorize] 
    [Authorize] 
    [Route("api/[controller]")] // Route de base: api/User
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // ... (WhoAmI et GetSenseiData restent protégés par [Authorize] de la classe) ...

        /// <summary>
        /// GET: api/User/testing-list - Retourne la liste de TOUS les utilisateurs.
        /// ATTENTION : Temporairement rendu public pour le développement grâce à [AllowAnonymous].
        /// </summary>
        [HttpGet("testing-list")]
        [AllowAnonymous] // <-- Permet l'accès sans JWT!
        public async Task<ActionResult> GetAllUsersForTesting()
        {
            // Récupère tous les utilisateurs de la base de données
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<object>();

            // Pour chaque utilisateur, on récupère son ID, nom d'utilisateur et ses rôles
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                
                userList.Add(new
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return Ok(userList);
        }
        
        /// <summary>
        /// GET: api/User/whoami - Retourne l'ID et les rôles de l'utilisateur connecté.
        /// </summary>
        [HttpGet("whoami")]
        public async Task<ActionResult> WhoAmI()
        {
            // Ce code est inchangé
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return Unauthorized("ID utilisateur non trouvé dans les jetons (claims).");
            }

            var userEntity = await _userManager.FindByIdAsync(userId);
            var roles = userEntity != null ? await _userManager.GetRolesAsync(userEntity) : Enumerable.Empty<string>();

            return Ok(new 
            { 
                UserId = userId, 
                Username = userEntity?.UserName,
                Roles = roles,
                Message = "Utilisateur authentifié" 
            });
        }

        /// <summary>
        /// GET: api/User/sensei-data - Endpoint accessible uniquement aux utilisateurs avec le rôle "Sensei".
        /// </summary>
        [HttpGet("sensei-data")]
        [Authorize(Roles = "Sensei")]
        public ActionResult<string> GetSenseiData()
        {
            return Ok("Accès autorisé. Vous pouvez maintenant accéder aux données réservées aux Sensei.");
        }
    }
}
