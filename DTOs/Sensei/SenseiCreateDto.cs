// using System.ComponentModel.DataAnnotations;

// namespace ASPPorcelette.API.DTOs.Sensei
// {
//     public class SenseiCreateDto
//     {
//         [Required(ErrorMessage = "Le nom est requis")]
//         [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
//         public string Nom { get; set; }

//         [Required(ErrorMessage = "Le prenom est requis")]
//         [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
//         public string Prenom { get; set; }

//         [Required(ErrorMessage = "L'email est requis")]
//         [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
//         public string Email { get; set; }

//         [Required(ErrorMessage = "Le téléphone est requis")]
//         [Phone(ErrorMessage = "Le numéro de téléphone n'est pas valide")]
//         public string Telephone { get; set; }
//         public string PhotoUrl { get; set; }

//         // Détails professionnels
//         [Required(ErrorMessage = "Le grade est requis")]
//         [MaxLength(50, ErrorMessage = "Le grade ne peut pas dépasser 50 caractères")]
//         public string Grade { get; set; }

//         [Required(ErrorMessage = "La bio est requise")]
//         [MaxLength(4000, ErrorMessage = "La bio ne peut pas dépasser 1000 caractères")]
//         public string Bio { get; set; }
//         public string Statut { get; set; }

//         // Clé étrangère pour la discipline principale
//         [Required(ErrorMessage = "La discipline principale est requise")]
//         public int DisciplineId { get; set; }
//     }
// }