using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Identity;

public class Actualite
{
    [Key]
    public int ActualiteId { get; set; }


    [Required(ErrorMessage = "Le titre est requis.")]
    [MaxLength(150)]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le contenu est requis.")]
    public string Contenu { get; set; } = string.Empty;

    [Required]
    public DateTime DateDePublication { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    // Événement associé (optionnel)
    public int? EvenementId { get; set; }

    [ForeignKey(nameof(EvenementId))]
    public virtual Evenement? EvenementAssocie { get; set; }
    // Clé étrangère vers User (Identity)
    [Required]
    public string UserId { get; set; } = null!;

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;


}
