using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ASPPorcelette.API.Models;

namespace ASPPorcelette.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>

    {
        // Le constructeur est obligatoire pour passer les options à DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // --- Déclaration des DbSet (Vos Tables) ---

        public DbSet<Actualite> Actualites { get; set; }
        public DbSet<Adherent> Adherents { get; set; }
        public DbSet<CategorieTransaction> CategorieTransactions { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Cours> Cours { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Evenement> Evenements { get; set; }
        public DbSet<Horaire> Horaires { get; set; }
        public DbSet<Sensei> Senseis { get; set; }
        public DbSet<Tarif> Tarifs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TypeEvenement> TypeEvenements { get; set; }
        public DbSet<User> User { get; set; }

        // ---------------------------------------------------------------------
        // CONFIGURATION DES RELATIONS COMPLEXES (OnModelCreating)
        // ---------------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                        // IMPORTANT : Toujours appeler la méthode de base pour gérer Identity
            base.OnModelCreating(modelBuilder);

            // Exemple de configuration : Relation plusieurs-à-plusieurs entre Adherent et Cours via une table de jonction
            modelBuilder.Entity<Adherent>()
                .HasMany(a => a.DisciplinesPratiquees)
                .WithMany(c => c.AdherentsApprenant)
                .UsingEntity(j => j.ToTable("Apprendre")); // Nom de la table de jonction

            // Ajoutez ici d'autres configurations complexes si nécessaire
        }


        // public DbSet<YourModel> YourModels { get; set; }
    }
}

