using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.User
{
    public class UpdateRoleRequestDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string NewRole { get; set; } = string.Empty;
    }
}