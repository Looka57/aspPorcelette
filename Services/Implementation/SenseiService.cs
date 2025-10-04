using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Assurez-vous que les using correspondent à l'emplacement de vos DTOs et Modèles
using ASPPorcelette.DTOs; // UserCreationDto (Si c'est le namespace racine)
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.Models; // Sensei/Adherent
using ASPPorcelette.API.Models.Identity; // User
using ASPPorcelette.API.Data; // ApplicationDbContext
using UserUpdateDto = ASPPorcelette.API.DTOs.User.UserUpdateDto; // Le DTO commun pour la mise à jour
using System;
using ASPPorcelette.API.DTOs;

namespace ASPPorcelette.API.Services
{
    // Note : L'interface ISenseiService est supposée définie ci-dessous.

    public class SenseiService : ISenseiService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Ajouté pour gestion des rôles si nécessaire
        private readonly ApplicationDbContext _context; // Votre DbContext

        public SenseiService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, // Injecter RoleManager est une bonne pratique
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

            // On s'assure que le rôle Adherent existe
            if (!await _roleManager.RoleExistsAsync("Adherent"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Adherent"));
            }
            // On s'assure que le rôle Sensei existe
            if (!await _roleManager.RoleExistsAsync("Sensei"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Sensei"));
            }


            // 2. CRÉATION ET LIAISON DU PROFIL MÉTIER (Adherent ou Sensei)
            string newUserId = user.Id;

            try
            {
                if (dto.IsSensei)
                {
                    // Utiliser le DTO Sensei spécifique ici si besoin, sinon UserCreationDto
                    var senseiProfile = new Sensei
                    {
                        UserId = newUserId,
                        Nom = dto.Nom,
                        Prenom = dto.Prenom,
                        Email = dto.Email,
                        Grade = dto.Grade, // Supposé exister sur UserCreationDto
                        Bio = dto.Bio // Supposé exister sur UserCreationDto
                    };

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
                        // Assurez-vous que Telephone/DateAdhesion sont gérés correctement s'ils sont dans le DTO
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
            catch (Exception ex)
            {
                // **POINT CRITIQUE : ANNULER LA CRÉATION USER EN CAS D'ÉCHEC DE LA BDD MÉTIER**
                // Si la sauvegarde du profil métier échoue (e.g., contrainte violée), on supprime 
                // l'utilisateur Identity pour éviter un compte orphelin.
                await _userManager.DeleteAsync(user);

                // Loguer l'erreur (ex) ici est fortement recommandé

                return IdentityResult.Failed(new IdentityError { Description = $"Erreur lors de la création du profil métier : {ex.Message}" });
            }
        }

        // ----------------------------------------------------------------------
        // MÉTHODE 2 : MISE À JOUR DU PROFIL COMMUN (Sensei OU Adherent)
        // ----------------------------------------------------------------------
        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, UserUpdateDto dto)
        {
            // 1. Trouver l'utilisateur Identity 
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Utilisateur Identity non trouvé." });
            }

            try
            {
                // Mise à jour des champs communs Identity (Nom/Prénom)
                user.Prenom = dto.Prenom ?? user.Prenom;
                user.Nom = dto.Nom ?? user.Nom;

                // Mise à jour de l'Username Identity
                if (!string.IsNullOrEmpty(dto.Username) && user.UserName != dto.Username)
                {
                    var usernameResult = await _userManager.SetUserNameAsync(user, dto.Username);
                    if (!usernameResult.Succeeded) return usernameResult;
                }

                // Mise à jour de l'Email Identity
                if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
                {
                    // L'email doit être géré avec un jeton si vous voulez un processus de confirmation
                    // Pour simplifier, on le change directement ici, mais la vérification est désactivée
                    user.Email = dto.Email;
                }

                var identityUpdateResult = await _userManager.UpdateAsync(user);
                if (!identityUpdateResult.Succeeded) return identityUpdateResult;


                // 3. Mise à jour spécifique au rôle dans la table métier
                var roles = await _userManager.GetRolesAsync(user);
                var isSensei = roles.Contains("Sensei");
                var isAdherent = roles.Contains("Adherent");

                if (isSensei)
                {
                    var senseiProfile = await _context.Senseis.FirstOrDefaultAsync(s => s.UserId == userId);

                    if (senseiProfile == null)
                    {
                        // Si le profil métier manque, on ne doit pas échouer l'update Identity, 
                        // mais on peut loguer un avertissement/erreur.
                        return IdentityResult.Failed(new IdentityError { Description = "Profil Sensei métier non trouvé." });
                    }

                    // Mapping des champs du DTO vers l'entité Sensei
                    senseiProfile.Prenom = dto.Prenom ?? senseiProfile.Prenom;
                    senseiProfile.Nom = dto.Nom ?? senseiProfile.Nom;
                    senseiProfile.Telephone = dto.Telephone;
                    senseiProfile.PhotoUrl = dto.PhotoUrl;
                    // Ajoutez ici les champs spécifiques à Sensei (Grade, Bio, etc.)
                    // senseiProfile.Grade = dto.Grade ?? senseiProfile.Grade; 
                    // senseiProfile.Bio = dto.Bio ?? senseiProfile.Bio; 

                    _context.Senseis.Update(senseiProfile);
                }
                else if (isAdherent)
                {
                    var adherentProfile = await _context.Adherents.FirstOrDefaultAsync(a => a.UserId == userId);

                    if (adherentProfile == null)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "Profil Adherent métier non trouvé." });
                    }

                    // Mapping des champs du DTO vers l'entité Adherent
                    adherentProfile.Prenom = dto.Prenom ?? adherentProfile.Prenom;
                    adherentProfile.Nom = dto.Nom ?? adherentProfile.Nom;
                    adherentProfile.Telephone = dto.Telephone;

                    _context.Adherents.Update(adherentProfile);
                }
                else
                {
                    // L'utilisateur est mis à jour dans Identity, mais aucun profil métier n'a été trouvé.
                    // On retourne le succès pour l'Identity, mais on pourrait retourner une alerte.
                }

                // 4. Sauvegarder les changements dans la base de données métier (Sensei/Adherent)
                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                // Gestion des erreurs générales
                return IdentityResult.Failed(new IdentityError { Description = $"Une erreur inattendue est survenue lors de la mise à jour du profil : {ex.Message}" });
            }
        }

        // ----------------------------------------------------
        // Implémentations des méthodes existantes (Stubs)
        // ----------------------------------------------------
        public Task<IEnumerable<ASPPorcelette.API.Models.Sensei>> GetAllSenseisAsync() => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei?> GetSenseiByIdAsync(int id) => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei> CreateSenseiAsync(ASPPorcelette.API.Models.Sensei sensei) => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei> UpdateSenseiAsync(ASPPorcelette.API.Models.Sensei sensei) => throw new NotImplementedException();
        public Task<(ASPPorcelette.API.Models.Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(int id, JsonPatchDocument<SenseiUpdateDto> patchDocument) => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei> DeleteSenseiAsync(int id) => throw new NotImplementedException();
    }
}
