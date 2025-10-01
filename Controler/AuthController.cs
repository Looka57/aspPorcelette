using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASPPorcelette.API.Models.Identity.Dto;
using ASPPorcelette.API.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ASPPorcelette.API.Services.Identity;
using ASPPorcelette.API.Models.DTOs;
using ASPPorcelette.API.DTOs; // Ajout nécessaire pour RegisterRequestDto

namespace ASPPorcelette.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Ajout pour vérifier/assigner les rôles

        public AuthController(IAuthService authService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // POST: api/Auth/Register
        /// <summary>
        /// Inscrit un nouvel utilisateur (par défaut: Student) et le connecte si l'opération réussit.
        /// </summary>
        [HttpPost("Register")]
        [AllowAnonymous] // Route publique pour l'inscription
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            // 1. Validation de l'état du modèle (vérifie les [Required] des DTO)
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDto
                {
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray(),
                    IsSuccess = false
                });
            }

            // 2. Vérification d'existence
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest(new AuthResultDto
                {
                    Errors = new[] { "L'utilisateur avec cet email existe déjà." },
                    IsSuccess = false
                });
            }

            // 3. Création de l'utilisateur
            User newUser = new User()
            {
                Email = model.Email,
                UserName = model.UserName, // Utilisez le champ UserName du DTO
                Nom = model.Nom,
                Prenom = model.Prenom,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                // Erreurs du UserManager (ex: le mot de passe ne respecte pas les règles de complexité)
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new AuthResultDto { Errors = errors, IsSuccess = false });
            }

            // 4. Attribution du rôle par défaut (ex: "Student")
            // REMARQUE: Assurez-vous que le rôle "Student" est bien créé dans AspNetRoles.
            const string defaultRole = "Student";

            // Vérifie si le rôle existe (si vous utilisez RoleManager)
            if (await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _userManager.AddToRoleAsync(newUser, defaultRole);
            }
            else
            {
                // Loguer un avertissement si le rôle n'existe pas.
                // Si vous ne voulez pas de RoleManager, vous pouvez enlever cette vérification,
                // mais la ligne suivante nécessite que le rôle existe.
                // await _userManager.AddToRoleAsync(newUser, defaultRole);
            }


            // 5. Tentative de connexion pour retourner le token immédiatement
            // Note: Nous utilisons les mêmes credentials pour LoginDto
            var loginModel = new LoginDto { Email = model.Email, Password = model.Password };
            var authResult = await _authService.LoginAsync(loginModel);

            if (authResult.IsSuccess)
            {
                return Ok(new AuthResultDto
                {
                    Token = authResult.Token,
                    IsSuccess = true,
                    UserId = authResult.UserId,
                    Roles = authResult.Roles // Assurez-vous que IAuthService.LoginAsync retourne les rôles
                });
            }

            // Si l'enregistrement a réussi mais que la connexion a échoué (très rare)
            return StatusCode(StatusCodes.Status500InternalServerError, new AuthResultDto
            {
                Errors = new[] { "Inscription réussie, mais échec de la connexion automatique." },
                IsSuccess = false
            });
        }


        // POST: api/Auth/Login
        /// <summary>
        /// Connecte un utilisateur (doit être un Sensei) et retourne un jeton JWT valide.
        /// </summary>
        [HttpPost("Login")]
        [AllowAnonymous] // L'accès est public pour la tentative de connexion
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _authService.LoginAsync(model);

            if (result.IsSuccess)
            {
                // Vérification supplémentaire: S'assurer que seul un utilisateur ayant le rôle Sensei peut se connecter ici
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = user != null ? await _userManager.GetRolesAsync(user) : new List<string>();

                if (!roles.Contains("Sensei") && !roles.Contains("Admin"))
                {
                    // Empêcher la connexion si l'utilisateur n'est pas un Sensei (même si le login/mdp est correct)
                    return Unauthorized(new AuthResultDto
                    {
                        Errors = new[] { "Accès refusé. Seuls les Sensei peuvent se connecter." },
                        IsSuccess = false
                    });
                }

                // Correction : Assurez-vous d'utiliser les informations complètes du résultat de LoginAsync
                return Ok(new AuthResultDto
                {
                    Token = result.Token,
                    IsSuccess = true,
                    UserId = result.UserId,
                    Roles = roles.ToList()
                });
            }

            return BadRequest(new AuthResultDto
            {
                Errors = result.Errors,
                IsSuccess = false
            });
        }
        
        [HttpPost("UpdateRole")]
    // Idéalement, cet endpoint devrait être sécurisé pour les Admins uniquement
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequestDto request)
    {
        // Validation basique pour s'assurer que le rôle est valide
        var allowedRoles = new List<string> { "Admin", "Student" }; 
        if (!allowedRoles.Contains(request.NewRole, StringComparer.OrdinalIgnoreCase))
        {
            return BadRequest(new { Message = $"Le rôle '{request.NewRole}' n'est pas supporté. Rôles permis : {string.Join(", ", allowedRoles)}." });
        }
        
        // Appelle le service pour mettre à jour le rôle
        var success = await _authService.UpdateUserRoleAsync(request.UserId, request.NewRole);

        if (success)
        {
            return Ok(new { Message = $"Rôle de l'utilisateur {request.UserId} mis à jour vers '{request.NewRole}' avec succès." });
        }
        else
        {
            return NotFound(new { Message = $"Utilisateur avec ID {request.UserId} non trouvé ou échec de la mise à jour." });
        }
    }
    }
}
