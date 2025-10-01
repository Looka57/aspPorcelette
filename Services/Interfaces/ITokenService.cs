using ASPPorcelette.API.Models.Identity;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Identity
{
    /// <summary>
    /// Définit le contrat pour la génération de tokens JWT après authentification.
    /// </summary>
    public interface ITokenService
    {
         Task<string> CreateTokenAsync(User user);
    }
}
