using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Assurez-vous que les using correspondent à l'emplacement de vos DTOs et Modèles
using ASPPorcelette.API.DTOs.User; 
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.Models.Identity; // User
using ASPPorcelette.API.Data; // ApplicationDbContext
using UserUpdateDto = ASPPorcelette.API.DTOs.User.UserUpdateDto; 
using System;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
// Peut être nécessaire pour les stubs (Sensei/Adherent)

namespace ASPPorcelette.API.Services
{
    // Note : L'interface ISenseiService est supposée définie ci-dessous.

    public class SenseiService : ISenseiService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context; // Maintenu pour les autres méthodes Sensei/Adherent si elles existent

        public SenseiService(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // ----------------------------------------------------
        // MÉTHODE 1 : Crée User (Identity) - Tous les champs sont sur l'objet User
        // ----------------------------------------------------
        public async Task<IdentityResult> CreateUserWithProfileAsync(UserCreationDto dto, string role)
        {
            // 1. CRÉATION DU COMPTE USER (Identity) - MAPPAGE DE TOUS LES CHAMPS DE PROFIL
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Nom = dto.Nom,
                Prenom = dto.Prenom,
                
                // --- CHAMPS DE PROFIL (maintenant tous dans l'entité User) ---
                Telephone = dto.Telephone,
                PhotoUrl = dto.PhotoUrl,
                Grade = dto.Grade,
                Bio = dto.Bio,
                Statut = dto.Statut ?? 0 , // Si Statut est géré par l'utilisateur
                
                // Champs d'adresse (pour Adherent et/ou Sensei)
                RueEtNumero = dto.Adresse, // Le champ 'Adresse' du DTO est mappé vers 'RueEtNumero'
                Ville = dto.Ville,
                CodePostal = dto.CodePostal,

                // Champs de dates et relations
                DateNaissance = dto.DateDeNaissance,
                DisciplineId = dto.DisciplineId,
                DateAdhesion = dto.DateAdhesion != default ? dto.DateAdhesion : DateTime.UtcNow,
                DateRenouvellement = dto.DateRenouvellement != default ? dto.DateRenouvellement : DateTime.UtcNow.AddYears(1)
            };

            // Création de l'utilisateur dans AspNetUsers
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return result;
            }
            
            // 2. VÉRIFICATION ET ATTRIBUTION DU RÔLE IDENTITY
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(role));
                if (!roleCreationResult.Succeeded)
                {
                    // Annuler la création user si le rôle ne peut être géré
                    await _userManager.DeleteAsync(user);
                    return roleCreationResult;
                }
            }

            // Attribution du rôle
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                // Annuler la création user si l'attribution du rôle échoue
                await _userManager.DeleteAsync(user);
                return roleResult;
            }
            
            // 3. RETRAIT DE LA LOGIQUE DE CRÉATION DE PROFIL SÉPARÉ
            // Les données de profil sont déjà sauvegardées avec _userManager.CreateAsync
            // et le rôle est assigné.

            return IdentityResult.Success;
        }

        // ----------------------------------------------------------------------
        // MÉTHODE 2 : MISE À JOUR DU PROFIL COMMUN (Unifié dans l'entité User)
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
                // Mise à jour des champs communs et spécifiques au profil
                user.Prenom = dto.Prenom ?? user.Prenom;
                user.Nom = dto.Nom ?? user.Nom;
                user.Telephone = dto.Telephone ?? user.Telephone; 
                user.PhotoUrl = dto.PhotoUrl ?? user.PhotoUrl;
                user.Grade = dto.Grade ?? user.Grade; 
                user.Bio = dto.Bio ?? user.Bio; 
                
                // Mise à jour de Statut (int? dans le DTO, int dans l'User)
                // Nous utilisons .HasValue pour vérifier si le champ a été envoyé
                if (dto.Statut.HasValue) 
                {
                    user.Statut = dto.Statut.Value;
                }
                
                // Mise à jour DisciplineId
                if (dto.DisciplineId.HasValue)
                {
                    user.DisciplineId = dto.DisciplineId.Value;
                }

                // Champs d'adresse
                user.RueEtNumero = dto.Adresse ?? user.RueEtNumero; 
                user.Ville = dto.Ville ?? user.Ville;
                user.CodePostal = dto.CodePostal ?? user.CodePostal;
                
                // Mise à jour DateNaissance
                if (dto.DateDeNaissance.HasValue)
                {
                    user.DateNaissance = dto.DateDeNaissance.Value;
                }

                // Champs Identity additionnels
                if (!string.IsNullOrEmpty(dto.Username) && user.UserName != dto.Username)
                {
                    var usernameResult = await _userManager.SetUserNameAsync(user, dto.Username);
                    if (!usernameResult.Succeeded) return usernameResult;
                }

                if (!string.IsNullOrEmpty(dto.Email) && user.Email != dto.Email)
                {
                    user.Email = dto.Email;
                }
                
                // --- CHANGEMENT DE MOT DE PASSE (Logique Identity) ---
                if (!string.IsNullOrEmpty(dto.CurrentPassword) && !string.IsNullOrEmpty(dto.NewPassword))
                {
                    var passwordChangeResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
                    if (!passwordChangeResult.Succeeded)
                    {
                        return passwordChangeResult;
                    }
                }
                // ---------------------------------------------------


                // 3. SAUVEGARDE DANS LA BASE DE DONNÉES (Uniquement via UserManager)
                var identityUpdateResult = await _userManager.UpdateAsync(user);
                
                return identityUpdateResult;
            }
            catch (Exception ex)
            {
                // Gestion des erreurs générales
                return IdentityResult.Failed(new IdentityError { Description = $"Une erreur inattendue est survenue lors de la mise à jour du profil : {ex.Message}" });
            }
        }

        // ----------------------------------------------------
        // Implémentations des méthodes existantes (Stubs)
        // Les stubs ci-dessous devront être mis à jour si les entités Sensei et Adherent 
        // ne sont plus utilisées du tout dans le modèle de données.
        // ----------------------------------------------------
        public Task<IEnumerable<ASPPorcelette.API.Models.Sensei>> GetAllSenseisAsync() => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei?> GetSenseiByIdAsync(int id) => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei> CreateSenseiAsync(ASPPorcelette.API.Models.Sensei sensei) => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei> UpdateSenseiAsync(ASPPorcelette.API.Models.Sensei sensei) => throw new NotImplementedException();
        public Task<(ASPPorcelette.API.Models.Sensei? Sensei, bool Success)> PartialUpdateSenseiAsync(int id, JsonPatchDocument<SenseiUpdateDto> patchDocument) => throw new NotImplementedException();
        public Task<ASPPorcelette.API.Models.Sensei> DeleteSenseiAsync(int id) => throw new NotImplementedException();
    }
}
