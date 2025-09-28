namespace ASPPorcelette.API.DTOs.Adherent
{
    public class AdherentDto
    {
        public int AdherentId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateAdhesion { get; set; }
        public DateTime DateRenouvellement { get; set; }
        public string Statut { get; set; }
    }
}


