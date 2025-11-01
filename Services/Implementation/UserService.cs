using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPPorcelette.API.DTOs.User; 
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Data;
using System;
using ASPPorcelette.API.DTOs;

namespace ASPPorcelette.API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IEnumerable<UserDto>> GetAdminUserListAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDtos = new List<UserDto>();
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Nom = user.Nom,
                    Prenom = user.Prenom,
                    Telephone = user.Telephone,
                    PhotoUrl = user.PhotoUrl,
                    DateDeCreation = user.DateCreation,
                    Grade = user.Grade,
                    Statut = user.Statut.ToString(),
                    RueEtNumero = user.RueEtNumero,
                    Ville = user.Ville,
                    CodePostal = user.CodePostal,
                    Bio = user.Bio,
                    DateNaissance = user.DateNaissance,
                    DateAdhesion = user.DateAdhesion,
                    DateRenouvellement = user.DateRenouvellement,
                    DisciplineId = user.DisciplineId,
                    Roles = roles.ToList()
                };
                userListDtos.Add(userDto);
            }

            return userListDtos;
        }

        private async Task<string> SaveProfilePicture(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return string.Empty;

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "profiles");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/profiles/{uniqueFileName}";
        }

        public async Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto, string role)
        {
            Console.WriteLine("=== Début de CreateUserWithProfileAsync ===");
            Console.WriteLine($"Email reçu : {dto.Email}");
            Console.WriteLine($"Date de naissance reçue : {dto.DateNaissance}");
            Console.WriteLine($"Fichier photo présent ? {(dto.PhotoFile != null ? "OUI" : "NON")}");
            
            string? photoUrl = await SaveProfilePicture(dto.PhotoFile);
            
            Console.WriteLine($"PhotoUrl enregistrée : {photoUrl}");
            Console.WriteLine($"DateNaissance reçue du DTO : {dto.DateNaissance}");
            
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                Telephone = dto.Telephone,
                PhotoUrl = photoUrl ?? string.Empty,
                Grade = dto.Grade ?? string.Empty,
                Bio = dto.Bio ?? string.Empty,
                Statut = dto.Statut ?? 0,
                RueEtNumero = dto.Adresse ?? string.Empty,
                Ville = dto.Ville ?? string.Empty,
                CodePostal = dto.CodePostal ?? string.Empty,
                DateNaissance = dto.DateNaissance,
                DisciplineId = dto.DisciplineId,
                DateAdhesion = dto.DateAdhesion != default ? dto.DateAdhesion : DateTime.UtcNow,
                DateRenouvellement = dto.DateRenouvellement != default ? dto.DateRenouvellement : DateTime.UtcNow.AddYears(1),
                DateCreation = DateTime.UtcNow
            };

            Console.WriteLine($"➡️ DateNaissance envoyée dans User : {user.DateNaissance}");
            Console.WriteLine($"➡️ PhotoUrl envoyée dans User : {user.PhotoUrl}");
            
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleCreationResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return roleCreationResult;
                }
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return roleResult;
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Utilisateur Identity non trouvé." });
            }

            try
            {
                user.Prenom = dto.Prenom ?? user.Prenom;
                user.Nom = dto.Nom ?? user.Nom;
                user.Telephone = dto.Telephone ?? user.Telephone; 
                user.PhotoUrl = dto.PhotoUrl ?? user.PhotoUrl;
                user.Grade = dto.Grade ?? user.Grade; 
                user.Bio = dto.Bio ?? user.Bio; 
                
                if (dto.Statut.HasValue) 
                {
                    user.Statut = dto.Statut.Value;
                }
                
                if (dto.DisciplineId.HasValue)
                {
                    user.DisciplineId = dto.DisciplineId.Value;
                }

                user.RueEtNumero = dto.Adresse ?? user.RueEtNumero; 
                user.Ville = dto.Ville ?? user.Ville;
                user.CodePostal = dto.CodePostal ?? user.CodePostal;
                
                if (dto.DateDeNaissance.HasValue)
                {
                    user.DateNaissance = dto.DateDeNaissance.Value;
                }

                if (!string.IsNullOrEmpty(dto.Username) && user.UserName != dto.Username)
                {
                    var usernameResult = await _userManager.SetUserNameAsync(user, dto.Username);
                    if (!usernameResult.Succeeded) return usernameResult;
                }

                if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
                {
                    user.Email = dto.Email;
                }
                
                if (!string.IsNullOrEmpty(dto.CurrentPassword) && !string.IsNullOrEmpty(dto.NewPassword))
                {
                    var passwordChangeResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                    if (!passwordChangeResult.Succeeded)
                    {
                        return passwordChangeResult;
                    }
                }

                var identityUpdateResult = await _userManager.UpdateAsync(user);
                
                return identityUpdateResult;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Une erreur inattendue est survenue lors de la mise à jour du profil : {ex.Message}" });
            }
        }

        // ❌ SUPPRIMEZ toutes ces méthodes NotImplementedException :
        // public Task<IEnumerable<ASPPorcelette.API.Models.Sensei>> GetAllSenseisAsync() => throw new NotImplementedException();
        // etc.
    }
}