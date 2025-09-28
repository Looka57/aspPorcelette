using System;

namespace ASPPorcelette.API.DTOs.Horaire
{
    public class HoraireDto
    {
        public int HoraireId { get; set; }
        public string Jour { get; set; } = string.Empty;
        public TimeSpan HeureDebut { get; set; }
        public TimeSpan HeureFin { get; set; }
        // Pas besoin de CoursId ici pour éviter les boucles, sauf si nécessaire pour le client.
    }
}
