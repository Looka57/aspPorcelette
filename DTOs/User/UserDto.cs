using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.DTOs.Sensei;

namespace ASPPorcelette.API.DTOs
{
    // Utilisé pour retourner les informations de base de l'utilisateur
    public class UserDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Prenom { get; set; } = string.Empty;

        [Required]
        public string Nom { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
        
        // Permet de savoir et d'afficher si cet utilisateur est un Sensei.
        public SenseiDto? Sensei { get; set; }
    }
}
