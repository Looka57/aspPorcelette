using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Assurez-vous que les using correspondent à l'emplacement de vos DTOs et Modèles
using ASPPorcelette.DTOs; // UserCreationDto
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Data;
using UserUpdateDto = ASPPorcelette.API.DTOs.User.UserUpdateDto; // Le DTO commun pour la mise à jour
using System;

namespace ASPPorcelette.API.Services
{
    public class SenseiService : ISenseiService 
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context; // Votre DbContext

        public SenseiService(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // ----------------------------------------------------
        // MÉTHODE 1 : Crée User (Identity) + Profil (EF Core)
        // ----------------------------------------------------
        public async Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto)
        {
            // 1. CRÉATION DU COMPTE USER (Identity)
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nom = dto.Nom,
                Prenom = dto.Prenom
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return result; 
            }

            // 2. CRÉATION ET LIAISON DU PROFIL MÉTIER (Adherent ou Sensei)
            string newUserId = user.Id;

            if (dto.IsSensei)
            {
                var senseiProfile = new Sensei
                {
                    UserId = newUserId, 
                    Nom = dto.Nom,
                    Prenom = dto.Prenom,
                    Email = dto.Email,
                    Grade = dto.Grade,
                    Bio = dto.Bio
                };

                // NOTE: J'utilise 'Senseis' ou 'Adherents' en supposant que ce sont les noms de vos DbSet.
                _context.Senseis.Add(senseiProfile); 
                await _userManager.AddToRoleAsync(user, "Sensei"); 
            }
            else
            {
                var adherentProfile = new Adherent
                {
                    UserId = newUserId, 
                    Nom = dto.Nom,
                    Prenom = dto.Prenom,
                    Email = dto.Email,
                    Telephone = dto.Telephone,
                    DateAdhesion = dto.DateAdhesion ?? DateTime.UtcNow 
                };

                _context.Adherents.Add(adherentProfile); 
                await _userManager.AddToRoleAsync(user, "Adherent"); 
            }

            // 3. SAUVEGARDE DANS LA BASE DE DONNÉES (DbContext)
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

        // ----------------------------------------------------------------------
        // NOUVELLE MÉTHODE 2 : MISE À JOUR DU PROFIL COMMUN (Sensei OU Adherent)
        // ----------------------------------------------------------------------
        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto dto)
        {
            try
            {
                // 1. Trouver l'utilisateur Identity pour obtenir son rôle
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Utilisateur Identity non trouvé." });
                }

                var roles = await _userManager.GetRolesAsync(user);
                var isSensei = roles.Contains("Sensei");
                var isAdherent = roles.Contains("Adherent");

                // 2. Mise à jour des champs communs (Nom/Prénom) dans l'objet Identity (si vous stockez ces champs là aussi)
                user.Prenom = dto.Prenom;
                user.Nom = dto.Nom;
                // Si l'Username est modifiable, mettez à jour l'Username Identity
                if (!string.IsNullOrEmpty(dto.Username) && user.UserName != dto.Username)
                {
                    var usernameResult = await _userManager.SetUserNameAsync(user, dto.Username);
                    if (!usernameResult.Succeeded) return usernameResult;
                }
                var identityUpdateResult = await _userManager.UpdateAsync(user);
                if (!identityUpdateResult.Succeeded) return identityUpdateResult;


                // 3. Mise à jour spécifique au rôle dans la table métier
                if (isSensei)
                {
                    var senseiProfile = await _context.Senseis.FirstOrDefaultAsync(s => s.UserId == userId);

                    if (senseiProfile == null)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "Profil Sensei non trouvé pour cet utilisateur." });
                    }

                    // Mapping des champs du DTO vers l'entité Sensei
                    senseiProfile.Prenom = dto.Prenom;
                    senseiProfile.Nom = dto.Nom;
                    senseiProfile.Telephone = dto.Telephone;
                    senseiProfile.PhotoUrl = dto.PhotoUrl; 
                    
                    _context.Senseis.Update(senseiProfile);
                }
                else if (isAdherent)
                {
                    var adherentProfile = await _context.Adherents.FirstOrDefaultAsync(a => a.UserId == userId);

                    if (adherentProfile == null)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "Profil Adherent non trouvé pour cet utilisateur." });
                    }

                    // Mapping des champs du DTO vers l'entité Adherent
                    adherentProfile.Prenom = dto.Prenom;
                    adherentProfile.Nom = dto.Nom;
                    // adherentProfile.Telephone = dto.Telephone;
                    // adherentProfile.PhotoUrl = dto.PhotoUrl;
                    
                    _context.Adherents.Update(adherentProfile);
                }
                else
                {
                    // L'utilisateur existe dans Identity, mais n'a pas de rôle Sensei/Adherent
                    return IdentityResult.Failed(new IdentityError { Description = "L'utilisateur n'a pas de rôle de profil reconnu (Sensei ou Adherent)." });
                }

                // 4. Sauvegarder les changements dans la base de données métier (Sensei/Adherent)
                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                 // Gestion des erreurs générales, y compris les erreurs de DbUpdateException
                return IdentityResult.Failed(new IdentityError { Description = $"Une erreur inattendue est survenue lors de la mise à jour du profil : {ex.Message}" });
            }
        }

        // ----------------------------------------------------
        // Implémentations des méthodes existantes (Stubs)
        // ----------------------------------------------------
        public Task<IEnumerable<Sensei>> GetAllSenseisAsync() => throw new NotImplementedException();
        public Task<Sensei?> GetSenseiByIdAsync(int id) => throw new NotImplementedException();
        public Task<Sensei> CreateSenseiAsync(Sensei sensei) => throw new NotImplementedException();
        public Task<Sensei> UpdateSenseiAsync(Sensei sensei) => throw new NotImplementedException();
        public Task<(Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(int id, JsonPatchDocument<SenseiUpdateDto> patchDocument) => throw new NotImplementedException();
        public Task<Sensei> DeleteSenseiAsync(int id) => throw new NotImplementedException();
    }
}
