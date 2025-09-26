using System;

namespace ASPPorcelette.API.Models
{
    public class Actualite
    {
        // Propriétés de la table (Colonnes)
        public int ActualiteId { get; set; }
        public string Titre { get; set; }
        public string Contenu { get; set; }
        public DateTime DateDePublication { get; set; } // Nom complet pour clarté
        public string ImageUrl { get; set; }
        // Note: La colonne 'auteur' n'est plus nécessaire car elle est représentée par SenseiId

        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------
        
        // 1. Clé vers le Sensei (l'auteur de l'actualité - relation 'annoncer'/'informer')
        public int SenseiId { get; set; }
        
        // 2. Clé vers l'Événement (l'actualité peut concerner un événement - relation 'informer')
        // Elle est nullable car l'actualité n'a pas toujours un lien avec un événement (cardinalité 0,n)
        public int? EvenementId { get; set; }


        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens avec d'autres classes)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails du Sensei qui a écrit l'actualité
        public Sensei SenseiAuteur { get; set; }

        // Permet d'accéder aux détails de l'événement associé (peut être null)
        public Evenement? EvenementAssocie { get; set; }
    }
}