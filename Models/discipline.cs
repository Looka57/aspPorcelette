using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Discipline
    {
        public int DisciplineId { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Navigation
        public ICollection<Cours> CoursAssocies { get; set; } = new List<Cours>();
        // public ICollection<Sensei> SenseisEnseignants { get; set; } = new List<Sensei>();
        public ICollection<Evenement> EvenementsAssocies { get; set; } = new List<Evenement>();
        public ICollection<Adherent> AdherentsApprenant { get; set; } = new List<Adherent>();
        public ICollection<Tarif> Tarifs { get; set; } = new List<Tarif>();
        public ICollection<Transaction> TransactionsAssociees { get; set; } = new List<Transaction>();

        // Navigation N-N via Apprendre
        public ICollection<Apprendre> Apprentissages { get; set; } = new List<Apprendre>();
    }
}
