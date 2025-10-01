namespace ASPPorcelette.API.Models.Identity.Dto
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        public string Email { get; set; } // Ajout pour l'email
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public string UserId { get; set; } = string.Empty; // Ajout pour l'ID utilisateur
        public List<string> Roles { get; set; } = new List<string>(); // Ajout pour les rôles
    }
}
