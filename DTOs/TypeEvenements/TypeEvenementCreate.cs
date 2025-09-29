// Fichier : DTOs/TypeEvenement/TypeEvenementCreateDto.cs
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.TypeEvenement
{
    public class TypeEvenementCreateDto
    {
        [Required(ErrorMessage = "Le libellé du type d'événement est requis.")]
        [StringLength(50)]
        public string Libelle { get; set; } = string.Empty;
    }
}