// using System.ComponentModel.DataAnnotations;
// using ASPPorcelette.API.DTOs.Discipline;

// namespace ASPPorcelette.API.DTOs.Sensei
// {
//   public class SenseiDto
//   {
//     [Required]
//     public int SenseiId { get; set; }
//     public string Nom { get; set; }
//     public string Prenom { get; set; }
//     public string Email { get; set; }
//     public string Telephone { get; set; }
//     public string PhotoUrl { get; set; }

//     // Détails professionnels
//     public string Grade { get; set; }
//     public string Bio { get; set; }
//     public string Statut { get; set; }

//     // Relation (utilise le DTO de la discipline pour éviter la boucle)
//     public DisciplineDto DisciplinePrincipale { get; set; }
//     public int DisciplineId { get; set; }
//     public string DisciplineNom { get; set; } = string.Empty; 
//           // Récupère l'ID de l'utilisateur Identity lié (pour référence)
//     [Required]
//     public string UserId { get; set; }

//     }
// }