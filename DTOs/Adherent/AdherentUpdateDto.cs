namespace ASPPorcelette.API.DTOs
{
    public class AdherentUpdateDto
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? Adresse { get; set; }
        public DateTime? DateDeNaissance { get; set; }
        public DateTime? DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; }
        public string? Statut { get; set; }
    }
}