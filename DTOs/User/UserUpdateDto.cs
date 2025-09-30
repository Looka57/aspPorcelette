using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.User
{
    // Utilisé pour la modification des informations de profil de base (Nom, Prénom, Téléphone)
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "Le prénom est requis.")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis.")]
        public string Nom { get; set; } = string.Empty;

        
        
        // Le nom d'utilisateur pourrait être optionnel si non modifiable
        public string? Username { get; set; }

        // Le numéro de téléphone est souvent modifiable
        [Phone(ErrorMessage = "Format de numéro de téléphone invalide.")]
        public string? Telephone { get; set; }
        
        // L'URL de la photo de profil (si nous laissons l'utilisateur la définir)
        public string? PhotoUrl { get; set; }
    }
}
