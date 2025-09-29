// Fichier : DTOs/TypeEvenement/TypeEvenementUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.TypeEvenement
{
    // DTO utilisé pour mettre à jour un TypeEvenement existant
    public class TypeEvenementUpdateDto
    {
        [Required(ErrorMessage = "Le libellé du type d'événement est requis.")]
        [StringLength(50)]
        public string Libelle { get; set; } = string.Empty;
    }
}