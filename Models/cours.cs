using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASPPorcelette.API.Models.Identity;
using Microsoft.AspNetCore.Identity; // Pour référencer la classe utilisateur si vous l'utilisez directement

namespace ASPPorcelette.API.Models
{
    public class Cours
    {
        public int CoursId { get; set; }
        public string Libelle { get; set; }

        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        // plus de Sensei
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Horaire> Horaires { get; set; }
    }
}