// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
// using ASPPorcelette.API.Models.Identity;

// namespace ASPPorcelette.API.Models
// {
//     public class Sensei
//     {
//         // Propriétés de la table (Colonnes)
//         [Key]
//         public int SenseiId { get; set; }

//         [Required]
//         [MaxLength(100)]
//         public string Nom { get; set; }

//         [Required]
//         [MaxLength(100)]
//         public string Prenom { get; set; }

//         [MaxLength(50)]
//         public string? Grade { get; set; }

//         [MaxLength(500)]
//         public string? Bio { get; set; }

//         [EmailAddress]
//         public string? Email { get; set; }

//         [Phone]
//         public string? Telephone { get; set; }

//         [MaxLength(500)]
//         public string? PhotoUrl { get; set; }

//         [MaxLength(50)]
//         public string? Statut { get; set; }

//         // -----------------------------------------------------------------
//         // Clés Étrangères (Foreign Keys)
//         // -----------------------------------------------------------------

//         // 1. Clé vers la Discipline principale (conforme à notre validation du MLD)
//         [ForeignKey("Discipline")]
//         public int DisciplineId { get; set; }

//         // 2. Clé vers le compte Utilisateur (pour la connexion et l'authentification)
//         public string? UserId { get; set; }


//         // -----------------------------------------------------------------
//         // Propriétés de Navigation (Liens)
//         // -----------------------------------------------------------------

//         // Permet d'accéder aux détails de la Discipline principale
//         public Discipline? DisciplinePrincipale { get; set; }

//         // Permet d'accéder au compte Utilisateur lié (peut être null)
//         public User? User { get; set; }

//         // Relation 1-à-N : Liste des cours que ce Sensei enseigne
//         public ICollection<Cours> CoursEnseignes { get; set; } = new List<Cours>();

//         // Relation 1-à-N : Liste des actualités publiées par ce Sensei
//         public ICollection<Actualite> ActualitesPubliees { get; set; } = new List<Actualite>();
//     }
// }