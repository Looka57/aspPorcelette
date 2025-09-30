using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models.Identity;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ASPPorcelette.API.Services.Identity
{
    // Implémentation des méthodes d'accès aux données utilisateur et de gestion de l'identité.
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // --- AUTHENTIFICATION & VÉRIFICATION ---

        public async Task<bool> IsUserExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            // Vérifie si le mot de passe fourni correspond au hash stocké
            // La méthode Verify gère la comparaison et le salage.
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                // Si PasswordHash est vide, la vérification échoue.
                return Task.FromResult(false);
            }
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
        }

        // --- OPÉRATIONS D'ÉCRITURE ---

        public async Task<User> CreateUserAsync(User user, string password)
        {
            // 1. Hacher le mot de passe avant de le stocker
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            
            // 2. Définir la date de création
            user.DateCreation = DateTime.Now;

            // 3. Ajouter à la base de données
            _context.Users.Add(userId);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            // Marque l'entité comme modifiée pour la persistance
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserPasswordAsync(User user, string newPassword)
        {
            // 1. Hacher le nouveau mot de passe
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            
            // 2. Mettre à jour l'entité
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
