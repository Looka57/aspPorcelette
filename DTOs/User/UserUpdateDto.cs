using System.ComponentModel.DataAnnotations;

namespace ASPPorcelette.API.DTOs.User
{
    // DTO utilisé pour la mise à jour du profil par l'utilisateur connecté.
    // Tous les champs sont optionnels car l'utilisateur ne les envoie pas tous à chaque fois.
    public class UserUpdateDto
    {
        // Champs Identity communs
        public string? Email { get; set; }
        public string? Username { get; set; } 
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Telephone { get; set; }

        // Mots de passe pour le changement de mot de passe (optionnel)
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
        
        // Champs spécifiques à Sensei (si vous les gérez via la même route d'update)
        public string? Bio { get; set; }
        public string? Grade { get; set; }
        // ... ajoutez d'autres champs spécifiques ici
    }
}
