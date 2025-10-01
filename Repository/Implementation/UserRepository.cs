// using ASPPorcelette.API.Data;
// using ASPPorcelette.API.Models.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Identity; // Nécessaire si vous héritez de IdentityDbContext

// namespace ASPPorcelette.API.Services.Identity
// {
//     // L'implémentation se concentre sur les accès aux données qui ne sont pas
//     // directement pris en charge par UserManager (ex: relations complexes, données spécifiques).
//     public class UserRepository : IUserRepository
//     {
//         // Note : Si vous utilisez IdentityDbContext, vous pouvez potentiellement accéder 
//         // aux utilisateurs via IdentityDbContext.Users ou simplement le DbContext.
//         private readonly ApplicationDbContext _context;

//         public UserRepository(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // --- AUTHENTIFICATION & VÉRIFICATION (Ces méthodes deviennent obsolètes ou inutiles car gérées par UserManager) ---

//         // Conserver uniquement si vous avez besoin d'une recherche DbContext-spécifique. 
//         // Sinon, utilisez UserManager.FindByEmailAsync().
//         public async Task<bool> IsUserExistAsync(string email)
//         {
//              // Accède directement à la table des utilisateurs via le DbContext 
//              // (Le DbContext doit hériter de IdentityDbContext<User>)
//             return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
//         }

//         public async Task<User?> GetUserByEmailAsync(string email)
//         {
//             // Conserver uniquement si vous avez besoin d'inclure des propriétés de navigation complexes.
//             return await _context.Users
//                                  .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
//         }

//         // Pas de FindAsync par int dans IdentityUser, il utilise un string pour l'ID.
//         // Si vous utilisez IdentityUser, l'ID est de type string.
//         public async Task<User?> GetUserByIdAsync(int id) 
//         {
//              // Supprimez cette méthode si IdentityUser.Id n'est pas un int.
//              // Si vous utilisez IdentityUser<int>, elle serait valide.
//              // Pour la sécurité, IdentityUser.Id est généralement string.
//              // Supposons que votre DbContext ait bien une DbSet<User> :
//              var userIdString = id.ToString(); // Conversion temporaire si l'ID est un string dans IdentityUser
//              return await _context.Users.FindAsync(userIdString);
//         }

//         // Cette méthode doit ÊTRE SUPPRIMÉE car le hachage est géré par Identity
//         public Task<bool> CheckPasswordAsync(User user, string password)
//         {
//              // !!! A Supprimer !!! Utiliser SignInManager.CheckPasswordSignInAsync() ou UserManager.CheckPasswordAsync()
//              // J'ajoute une exception pour signaler que cette méthode est erronée.
//              throw new NotSupportedException("CheckPasswordAsync n'est pas supporté. Utilisez UserManager.CheckPasswordAsync() à la place.");
//         }

//         // --- OPÉRATIONS D'ÉCRITURE ---

//         // Cette méthode doit ÊTRE SUPPRIMÉE car la création est gérée par Identity
//         public Task<User> CreateUserAsync(User user, string password)
//         {
//              // !!! A Supprimer !!! Utiliser UserManager.CreateAsync()
//              throw new NotSupportedException("CreateUserAsync n'est pas supporté. Utilisez UserManager.CreateAsync() à la place.");
//         }

//         public async Task<bool> UpdateUserAsync(User user)
//         {
//             // Si vous ne mettez à jour que des champs personnalisés, c'est utile.
//             // Sinon, utilisez UserManager.UpdateAsync(user).
//             _context.Users.Update(user);
//             await _context.SaveChangesAsync();
//             return true;
//         }

//         // Cette méthode doit ÊTRE SUPPRIMÉE car la mise à jour du mot de passe est gérée par Identity
//         public Task<bool> UpdateUserPasswordAsync(User user, string newPassword)
//         {
//              // !!! A Supprimer !!! Utiliser UserManager.ChangePasswordAsync() ou UserManager.RemovePasswordAsync()/AddPasswordAsync()
//              throw new NotSupportedException("UpdateUserPasswordAsync n'est pas supporté. Utilisez UserManager.ChangePasswordAsync() à la place.");
//         }
//     }
// }
