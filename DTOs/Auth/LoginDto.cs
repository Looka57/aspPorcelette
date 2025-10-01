using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.Models.Identity.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
