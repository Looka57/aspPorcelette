// DTO de Création (POST)
using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Enums;

// DTO de Création (POST)
public class CategorieTransactionCreateDto
    {
        [Required]
        [MaxLength(150)]
        public string Nom { get; set; } = string.Empty;

        // L'enum garantit que seule une valeur valide est acceptée
        [Required(ErrorMessage = "Le type de flux (Recette ou Dépense) est requis.")]
        public TypeFlux TypeFlux { get; set; }
    }