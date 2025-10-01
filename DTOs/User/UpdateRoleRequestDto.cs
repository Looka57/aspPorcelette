using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs
{
    public class UpdateRoleRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string NewRole { get; set; }
    }
}
