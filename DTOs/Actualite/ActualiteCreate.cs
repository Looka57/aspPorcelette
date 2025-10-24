// DTO de Création (Entrée POST)
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // 👈 NÉCESSAIRE POUR IFormFile

public class ActualiteCreateDto
{
    [Required(ErrorMessage = "Le titre est requis.")]
    [StringLength(150)]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le contenu est requis.")]
    public string Contenu { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de publication est requise.")]
    public DateTime DatePublication { get; set; }
    public int? EvenementId { get; set; }


    // 🎯 ESSENTIEL : Le champ qui reçoit le fichier binaire.
    // Le nom 'ImageFile' doit correspondre à la clé utilisée dans FormData côté Vue.
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "L'ID de l'utilisateur (auteur) est requis.")]
    public string UserId { get; set; } = string.Empty;

    public string? ImageUrl { get; set; } // 🎯 NOUVEAU/RÉINTRODUIT

}