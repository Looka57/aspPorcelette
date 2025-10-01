using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.Models.DTOs
{
    /// <summary>
    /// DTO pour la requête d'enregistrement.
    /// </summary>
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom de famille ou nom complet est requis.")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prenom  est requis.")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir entre 6 et 100 caractères.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO pour la requête de connexion.
    /// </summary>
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO pour la réponse de connexion/enregistrement (contenant le JWT).
    /// </summary>
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
