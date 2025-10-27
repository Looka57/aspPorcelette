using System.ComponentModel.DataAnnotations;
using ASPPorcelette.API.Enums;

public class CompteCreateDto
{
    [Required]
    [MaxLength(150)]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le type de compte (Courant ou Epargne) est requis.")]
    public TypeCompte TypeCompte { get; set; }

    [Required]
    [Range(typeof(decimal), "-999999.99", "999999.99")]
    public decimal Solde { get; set; } = 0.00m;

    [Required]
    [Range(typeof(decimal), "0.00", "999999.99")]
    public decimal Epargne { get; set; } = 0.00m;
}
