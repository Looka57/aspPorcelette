using System;
using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Adherent
    {
        public int AdherentId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; }
        public string Statut { get; set; }

        // Navigation N-N via Apprendre
        public ICollection<Apprendre> Apprentissages { get; set; } = new List<Apprendre>();
        public ICollection<Discipline> DisciplinesPratiquees { get; set; } = new List<Discipline>();
    }
}
