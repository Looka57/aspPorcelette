using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

public class ActualiteUpdateDto
{
    [Required(ErrorMessage = "Le titre est requis.")]
    [StringLength(150)]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le contenu est requis.")]
    public string Contenu { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de publication est requise.")]
    public DateTime DatePublication { get; set; }

    // Image facultative
    public IFormFile? ImageFile { get; set; }

    // ID de l'événement associé, facultatif
    public int? EvenementId { get; set; }

    // Supprimer l'image existante si true
    public bool DeleteExistingImage { get; set; } = false;
}
