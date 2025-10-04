using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.User
{
    /// <summary>
    /// DTO pour l'attribution/retrait de rôles
    /// </summary>
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "L'identifiant utilisateur est requis.")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom du rôle est requis.")]
        public string RoleName { get; set; } = string.Empty;
    }
}