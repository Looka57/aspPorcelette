// Fichier : DTOs/TypeEvenement/TypeEvenementDto.cs
namespace ASPPorcelette.API.DTOs.TypeEvenement
{
    public class TypeEvenementDto
    {
        public int TypeEvenementId { get; set; }
        public string Libelle { get; set; } = string.Empty;
    }
}