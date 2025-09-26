using System;
using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Adherent
    {
        // Propriétés de la table (Colonnes)
        public int AdherentId { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime DateDeNaissance { get; set; } // Nom complet du MLD
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateAdhesion { get; set; }
        public DateTime? DateRenouvellement { get; set; } // Rendu nullable pour les nouveaux membres

        // --- Gestion du Statut (Unification) ---
        // La colonne 'Statut' dans votre MLD doit correspondre à une seule propriété pour la cohérence.
        public string Statut { get; set; } // Par exemple: "Actif", "En Attente", "Inactif"

        // Supprimez 'EstActif' et 'Statut' si elles sont toutes deux des bool.
        // Si Statut est un bool, utilisez plutôt une seule propriété claire :
        // public bool EstActif { get; set; } 
        // ----------------------------------------


        // -----------------------------------------------------------------
        // Propriétés de Navigation (Relation N,N Adherent-Discipline 'apprendre')
        // -----------------------------------------------------------------
        
        // Le nom reflète l'association 'apprendre' ou 'pratiquer'
        public ICollection<Discipline> DisciplinesPratiquees { get; set; } = new List<Discipline>();
    }
}