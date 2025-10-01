using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.Models.Identity.Dto
{
    // Ce DTO est utilisé par le Sensei pour créer un utilisateur en spécifiant le rôle.
    public class RegisterWithRoleDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        // Le rôle doit être 'Student' ou 'Sensei'
        public string Role { get; set; }
    }
}
