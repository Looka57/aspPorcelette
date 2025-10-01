using System;
using System.Collections.Generic;
using ASPPorcelette.API.Models.Identity;
using Microsoft.AspNetCore.Identity; // <-- NÉCESSAIRE POUR LA PROPRIÉTÉ DE NAVIGATION

namespace ASPPorcelette.API.Models
{
    public class Adherent
    {
        public int AdherentId { get; set; }

        // **********************************************
        // CLÉ ÉTRANGÈRE VERS IDENTITY USER
        // **********************************************
        public string? UserId { get; set; }
        // Propriété de navigation optionnelle, mais bonne pratique.
        public User User { get; set; }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; } // Gardez-le si c'est pour l'historique ou le contact rapide
        public string Telephone { get; set; }
        public DateTime DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; }
        public string Statut { get; set; }

        // Navigation N-N
        public ICollection<Apprendre> Apprentissages { get; set; } = new List<Apprendre>();
        public ICollection<Discipline> DisciplinesPratiquees { get; set; } = new List<Discipline>();
    }
}
