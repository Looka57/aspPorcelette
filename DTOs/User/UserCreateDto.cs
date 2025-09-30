using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.User
{
    // Utilisé pour l'enregistrement (inscription) d'un nouvel utilisateur
    public class UserCreateDto
    {
        // --- Informations d'Authentification ---
        
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "Format d'adresse e-mail invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, ErrorMessage = "Le mot de passe doit contenir au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // --- Informations de Profil ---
        
        [Required(ErrorMessage = "Le prénom est requis.")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est requis.")]
        public string Nom { get; set; } = string.Empty;
        
        // Le nom d'utilisateur est souvent utilisé comme pseudo, peut être optionnel ou requis selon la politique
        public string? Username { get; set; }

        [Phone(ErrorMessage = "Format de numéro de téléphone invalide.")]
        public string? Telephone { get; set; }
    }
}
