using System;

namespace ASPPorcelette.API.Models
{
    public class Transaction
    {
        // Propriétés de la table (Colonnes)
        public int TransactionId { get; set; }
        public decimal Montant { get; set; } // Type recommandé pour la comptabilité
        public DateTime DateTransaction { get; set; }
        public string Description { get; set; }

        // -----------------------------------------------------------------
        // Clés Étrangères (Foreign Keys)
        // -----------------------------------------------------------------
        
        // 1. Clé vers le Compte (Relation 1,1 côté Compte)
        public int CompteId { get; set; }
        
        // 2. Clé vers la Catégorie (Relation 1,1 côté CatégorieTransaction)
        public int CategorieTransactionId { get; set; }

        // 3. Clé vers la Discipline (Relation 0,n côté Discipline : Rendue nullable)
        public int? DisciplineId { get; set; } 
        
        
        // -----------------------------------------------------------------
        // Propriétés de Navigation (Liens vers les Objets)
        // -----------------------------------------------------------------
        
        // Permet d'accéder aux détails du Compte associé
        public Compte Compte { get; set; }

        // Permet d'accéder aux détails de la Catégorie associée
        public CategorieTransaction Categorie { get; set; }

        // Permet d'accéder aux détails de la Discipline associée (peut être null)
        public Discipline? DisciplineAssociee { get; set; }
    }
}