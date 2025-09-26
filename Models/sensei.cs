using System.Collections.Generic;

namespace ASPPorcelette.API.Models
{
    public class Sensei
    {
        // Propriétés de la table (Colonnes)
        public int SenseiId { get; set; } // Renommé pour la cohérence du modèle
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Grade { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string PhotoUrl { get; set; }
        public string Statut { get; set; } 

        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------
        
        // 1. Clé vers la Discipline principale (conforme à notre validation du MLD)
        public int DisciplineId { get; set; }
        
        // 2. Clé vers le compte Utilisateur (pour la connexion et l'authentification)
        public string? UtilisateurId { get; set; } 

        
        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails de la Discipline principale
        public Discipline DisciplinePrincipale { get; set; }

        // Permet d'accéder au compte Utilisateur lié (peut être null)
        public User? Utilisateur { get; set; } 

        // Relation 1-à-N : Liste des cours que ce Sensei enseigne
        public ICollection<Cours> CoursEnseignes { get; set; } = new List<Cours>();

        // Relation 1-à-N : Liste des actualités publiées par ce Sensei
        public ICollection<Actualite> ActualitesPubliees { get; set; } = new List<Actualite>();
    }
}