using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.User
{
    // Utilisé pour gérer la modification de l'adresse e-mail de l'utilisateur.
    // Cette opération doit requérir le mot de passe actuel pour des raisons de sécurité.
    public class UserEmailUpdateDto
    {
        [Required(ErrorMessage = "L'adresse e-mail actuelle est requise pour confirmation.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nouvelle adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "Format d'adresse e-mail invalide.")]
        public string NewEmail { get; set; } = string.Empty;
    }
}
