namespace ASPPorcelette.API.Models.Identity.Dto
{
    public class UserInfoDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        // Nouvelle propriété pour stocker les rôles de l'utilisateur
        public List<string> Roles { get; set; } = new List<string>(); 
    }
}
