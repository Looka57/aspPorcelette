using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPPorcelette.API.DTOs.User;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Data;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using ASPPorcelette.API.DTOs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

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

        // ======================================================================
        // 🔹 Lister les utilisateurs
        // ======================================================================
        public async Task<IEnumerable<UserDto>> GetAdminUserListAsync()
        {
            var users = await _userManager.Users
            .Where(u => u.Statut == 1)
            .ToListAsync();

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
                    // certificat medicale
                    CertificatMedicalFourni = user.CertificatMedicalFourni,
                    DateCertificatMedical = user.DateCertificatMedical,
                    DateExpirationCertificatMedical = user.DateExpirationCertificatMedical,
                    Roles = roles.ToList(),
                };
                userListDtos.Add(userDto);
            }

            return userListDtos;
        }

        // ======================================================================
        // 🔹 Compter les adhérents actifs (Statut = 1 et DateRenouvellement >= aujourd'hui)
        // ======================================================================
        public async Task<int> GetActiveAdherentsCountAsync()
        {
            var today = DateTime.Today;

            // Saison : du 1er septembre au 30 juin
            int startYear = today.Month < 9
                ? today.Year - 1
                : today.Year;

            DateTime cycleStart = new DateTime(startYear, 9, 1);
            DateTime cycleEnd = new DateTime(startYear + 1, 6, 30);

            return await _userManager.Users
                .Where(u =>
                    u.Statut == 1 &&
                    u.DateRenouvellement.HasValue &&
                    u.DateRenouvellement.Value >= cycleStart &&
                    u.DateRenouvellement.Value <= cycleEnd)
                .CountAsync();
        }

        // ======================================================================
        // 🔹 Date adhesion l'annee suivante
        // ======================================================================
        private DateTime GetStartOfNextAdhesionCycle()
        {
            var today = DateTime.Today;
            int year = today.Month < 9 ? today.Year : today.Year + 1;
            return new DateTime(year, 9, 1);
        }

        // ======================================================================
        // 🔹 Applique la date du certificat médical et calcule automatiquement
        //     sa date d'expiration (+3 ans). Utilisée par les 3 points d'entrée
        //     (création, mise à jour profil, mise à jour admin) pour garantir
        //     un comportement identique partout.
        // ======================================================================
        // ======================================================================
        // CERTIFICAT MEDICAL
        // ======================================================================

        private void ApplyCertificatMedical(
     User user,
     DateTime? dateCertificat,
     bool? certificatFourniOverride = null)
        {
            // Si une date de certificat est fournie
            if (dateCertificat.HasValue)
            {
                var dateCertificatDate = dateCertificat.Value.Date;

                // Date du certificat
                user.DateCertificatMedical = dateCertificatDate;

                // Expiration = certificat + 3 ans
                user.DateExpirationCertificatMedical =
                    dateCertificatDate.AddYears(3);

                // Premier rappel = 1 mois avant l'expiration
                user.DateRappelCertificatMedical =
                    user.DateExpirationCertificatMedical.Value.AddMonths(-1);

                // Certificat fourni
                user.CertificatMedicalFourni = true;
            }
            else if (certificatFourniOverride == false)
            {
                // Aucun certificat
                user.CertificatMedicalFourni = false;
                user.DateCertificatMedical = null;
                user.DateExpirationCertificatMedical = null;
                user.DateRappelCertificatMedical = null;
            }
            else if (certificatFourniOverride.HasValue)
            {
                user.CertificatMedicalFourni = certificatFourniOverride.Value;
            }
        }

        // ======================================================================
        // 🔹 Renouveler l'adhésion d'un utilisateur
        // ======================================================================
        public async Task<IdentityResult> RenewAdhesionAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Utilisateur non trouvé." });

            //   / La date de renouvellement est le 31 août de l'année suivante
            DateTime nextCycleStart = GetStartOfNextAdhesionCycle();
            user.DateRenouvellement = nextCycleStart.AddDays(-1); // 31 août

            user.Statut = 1; // actif

            return await _userManager.UpdateAsync(user);
        }


        // ======================================================================
        // 🔹 Sauvegarder une image sur disque
        // ======================================================================
        private async Task<string> SaveProfilePicture(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return string.Empty;

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images", "profiles");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + ".webp";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                // 3. Conversion réelle en WebP via ImageSharp
                using (var stream = imageFile.OpenReadStream())
                {
                    using (var image = await Image.LoadAsync(stream))
                    {
                        // Optionnel : Redimensionner les photos de profil si elles sont trop grandes (ex: 500x500)
                        // image.Mutate(x => x.Resize(new ResizeOptions { Size = new Size(500, 500), Mode = ResizeMode.Max }));

                        var encoder = new WebpEncoder { Quality = 80 };
                        await image.SaveAsync(filePath, encoder);
                    }
                }

                Console.WriteLine($"✅ Photo de profil convertie et sauvegardée: {filePath}");
                return $"/images/profiles/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de la conversion de la photo de profil : {ex.Message}");
                return string.Empty;
            }
        }

        // ======================================================================
        // 🔹 Supprimer une image du disque (VERSION CORRIGÉE)
        // ======================================================================
        private void DeleteProfilePicture(string? photoUrl)
        {
            if (string.IsNullOrEmpty(photoUrl))
                return;

            try
            {
                // ✅ Enlève un éventuel "/" au début
                string relativePath = photoUrl.StartsWith("/") ? photoUrl.TrimStart('/') : photoUrl;

                // ✅ Remplace les "/" par le séparateur de répertoire approprié (Windows ou Linux)
                relativePath = relativePath.Replace("/", Path.DirectorySeparatorChar.ToString());

                // ✅ Combine correctement le chemin complet
                string fullPath = Path.Combine(_hostEnvironment.WebRootPath, relativePath);

                Console.WriteLine($"🔍 Tentative de suppression : {fullPath}");

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    Console.WriteLine($"✅ Fichier supprimé avec succès : {fullPath}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Fichier introuvable : {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de la suppression de l'image {photoUrl} : {ex.Message}");
                Console.WriteLine($"❌ Stack trace : {ex.StackTrace}");
            }
        }

        // ======================================================================
        // 🔹 Création d'un utilisateur avec photo
        // ======================================================================
        public async Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto, string role)
        {
            string? photoUrl = await SaveProfilePicture(dto.PhotoFile);

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
                RueEtNumero = dto.RueEtNumero ?? string.Empty,
                Ville = dto.Ville ?? string.Empty,
                CodePostal = dto.CodePostal ?? string.Empty,
                DateNaissance = dto.DateNaissance,
                DisciplineId = dto.DisciplineId,
                DateAdhesion = dto.DateAdhesion != default ? dto.DateAdhesion : DateTime.UtcNow,
                DateRenouvellement = dto.DateRenouvellement != default ? dto.DateRenouvellement : DateTime.UtcNow.AddYears(1),

                DateCreation = DateTime.UtcNow
            };

            // === CERTIFICAT MÉDICAL === (calcul auto de la date d'expiration si une date est fournie)
            ApplyCertificatMedical(user, dto.DateCertificatMedical, dto.CertificatMedicalFourni);

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return result;

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

        // ======================================================================
        // 🔹 Mise à jour du profil utilisateur (par lui-même)
        // ======================================================================
        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Utilisateur non trouvé." });

            try
            {
                user.Prenom = dto.Prenom ?? user.Prenom;
                user.Nom = dto.Nom ?? user.Nom;
                user.Telephone = dto.Telephone ?? user.Telephone;
                user.Grade = dto.Grade ?? user.Grade;
                user.Bio = dto.Bio ?? user.Bio;

                // === Gestion de la photo ===
                if (dto.PhotoFile != null)
                {
                    string newPhotoUrl = await SaveProfilePicture(dto.PhotoFile);
                    if (!string.IsNullOrEmpty(newPhotoUrl))
                    {
                        string? oldPhotoUrl = user.PhotoUrl;
                        user.PhotoUrl = newPhotoUrl;
                        DeleteProfilePicture(oldPhotoUrl); // 🟢 supprime l'ancienne image
                    }
                }
                else if (dto.PhotoUrl == string.Empty && !string.IsNullOrEmpty(user.PhotoUrl))
                {
                    DeleteProfilePicture(user.PhotoUrl);
                    user.PhotoUrl = null;
                }
                else if (dto.PhotoFile == null && !string.IsNullOrEmpty(dto.PhotoUrl))
                {
                    user.PhotoUrl = dto.PhotoUrl;
                }

                if (dto.Statut.HasValue) user.Statut = dto.Statut.Value;
                if (dto.DisciplineId.HasValue) user.DisciplineId = dto.DisciplineId.Value;
                user.RueEtNumero = dto.RueEtNumero ?? user.RueEtNumero;
                user.Ville = dto.Ville ?? user.Ville;
                user.CodePostal = dto.CodePostal ?? user.CodePostal;

                if (dto.DateDeNaissance.HasValue) user.DateNaissance = dto.DateDeNaissance.Value.Date;
                if (dto.DateAdhesion.HasValue) user.DateAdhesion = dto.DateAdhesion.Value;
                if (dto.DateRenouvellement.HasValue) user.DateRenouvellement = dto.DateRenouvellement.Value;

                // === CERTIFICAT MÉDICAL === (calcul auto de la date d'expiration si une date est fournie)
                ApplyCertificatMedical(user, dto.DateCertificatMedical, dto.CertificatMedicalFourni);

                if (!string.IsNullOrEmpty(dto.Username) && user.UserName != dto.Username)
                {
                    var usernameResult = await _userManager.SetUserNameAsync(user, dto.Username);
                    if (!usernameResult.Succeeded) return usernameResult;
                }

                if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
                    user.Email = dto.Email;

                // === Changement de mot de passe ===
                if (!string.IsNullOrEmpty(dto.NewPassword))
                {
                    if (string.IsNullOrEmpty(dto.CurrentPassword))
                        return IdentityResult.Failed(new IdentityError { Description = "Le mot de passe actuel est requis." });

                    var passwordCheck = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
                    if (!passwordCheck)
                        return IdentityResult.Failed(new IdentityError { Description = "Mot de passe actuel incorrect." });

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);
                    if (!passwordResult.Succeeded) return passwordResult;
                }

                return await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Erreur lors de la mise à jour du profil : {ex.Message}"
                });
            }
        }

        // ======================================================================
        // 🔹 Mise à jour d'un utilisateur par un admin
        // ======================================================================
        public async Task<IdentityResult> UpdateUserByAdminAsync(string userId, UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "Utilisateur non trouvé." });

            user.Prenom = dto.Prenom ?? user.Prenom;
            user.Nom = dto.Nom ?? user.Nom;
            user.Telephone = dto.Telephone ?? user.Telephone;
            user.Grade = dto.Grade ?? user.Grade;
            user.Bio = dto.Bio ?? user.Bio;

            user.RueEtNumero = dto.RueEtNumero ?? user.RueEtNumero;
            user.Ville = dto.Ville ?? user.Ville;
            user.CodePostal = dto.CodePostal ?? user.CodePostal;

            if (dto.Statut.HasValue) user.Statut = dto.Statut.Value;
            if (dto.DisciplineId.HasValue) user.DisciplineId = dto.DisciplineId.Value;
            if (dto.DateDeNaissance.HasValue)
            {
                user.DateNaissance = dto.DateDeNaissance.Value.Date;
            }

            if (dto.DateAdhesion.HasValue) user.DateAdhesion = dto.DateAdhesion.Value;
            if (dto.DateRenouvellement.HasValue) user.DateRenouvellement = dto.DateRenouvellement.Value;

            // =============================
            // CERTIFICAT MÉDICAL
            // =============================

            // =============================
            // CERTIFICAT MÉDICAL
            // =============================

            ApplyCertificatMedical(
                user,
                dto.DateCertificatMedical,
                dto.CertificatMedicalFourni
            );
            // === Gestion de la photo ===
            if (dto.PhotoFile != null)
            {
                string newPhotoUrl = await SaveProfilePicture(dto.PhotoFile);
                if (!string.IsNullOrEmpty(newPhotoUrl))
                {
                    string? oldPhotoUrl = user.PhotoUrl;
                    user.PhotoUrl = newPhotoUrl;
                    DeleteProfilePicture(oldPhotoUrl);
                }
            }
            else if (dto.PhotoUrl == string.Empty && !string.IsNullOrEmpty(user.PhotoUrl))
            {
                DeleteProfilePicture(user.PhotoUrl);
                user.PhotoUrl = null;
            }
            else if (dto.PhotoFile == null && !string.IsNullOrEmpty(dto.PhotoUrl))
            {
                user.PhotoUrl = dto.PhotoUrl;
            }

            if (!string.IsNullOrEmpty(dto.Username) && user.UserName != dto.Username)
            {
                var usernameResult = await _userManager.SetUserNameAsync(user, dto.Username);
                if (!usernameResult.Succeeded) return usernameResult;
            }

            if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
                user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded) return removeResult;

                var addResult = await _userManager.AddPasswordAsync(user, dto.NewPassword);
                if (!addResult.Succeeded) return addResult;
            }

            return await _userManager.UpdateAsync(user);
        }


        // ======================================================================
        // 🔹 Désactivation d'un utilisateur (Soft Delete : Statut = 0)
        // ======================================================================
        public async Task<IdentityResult> DeactivateUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // Si l'utilisateur n'existe pas, l'opération est considérée comme réussie pour l'état final.
            if (user == null)
                return IdentityResult.Success;

            // 🎯 MISE EN PLACE DE LA SUPPRESSION DOUCE
            // 1. Définir le Statut à 0 (Inactif)
            user.Statut = 0;

            // 2. Mettre à jour l'utilisateur dans la base de données
            var result = await _userManager.UpdateAsync(user);

            // 🛑 La photo est CONSERVÉE sur le disque, comme demandé.
            // L'appel à DeleteProfilePicture est omis ici.

            return result;
        }
    }
}