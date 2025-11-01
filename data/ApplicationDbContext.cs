using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Identity;

namespace ASPPorcelette.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Actualite> Actualites { get; set; }
        public DbSet<Apprendre> Apprendre { get; set; }
        public DbSet<Adherent> Adherents { get; set; }
        public DbSet<CategorieTransaction> CategorieTransactions { get; set; }
        public DbSet<Compte> Comptes { get; set; }
        public DbSet<Cours> Cours { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Evenement> Evenements { get; set; }
        public DbSet<Horaire> Horaires { get; set; }
        // public DbSet<Sensei> Senseis { get; set; }
        // public DbSet<Sensei> SenseiProfiles { get; set; } 
        public DbSet<Tarif> Tarifs { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TypeEvenement> TypeEvenements { get; set; }
        // public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // L'Adhérent est le profil détaillé associé au compte User (AspNetUsers)
            modelBuilder.Entity<Adherent>()
                .HasOne(a => a.User) // L'Adherent a une propriété de navigation vers User
                .WithOne() // Relation One-to-One sans propriété de navigation inverse dans User
                .HasForeignKey<Adherent>(a => a.UserId) // La clé étrangère se trouve dans la table Adherent
                .IsRequired(false);

            // Relation N-N Adherent ↔ Discipline via Apprendre
            modelBuilder.Entity<Apprendre>()
            .ToTable("Apprendre")
                .HasKey(a => new { a.AdherentId, a.DisciplineId }); // clé composite

            modelBuilder.Entity<Apprendre>()
                .HasOne(a => a.AdherentApprenant)
                .WithMany(ad => ad.Apprentissages)
                .HasForeignKey(a => a.AdherentId);

            modelBuilder.Entity<Apprendre>()
                .HasOne(a => a.DisciplinePratiquee)
                .WithMany(d => d.Apprentissages)
                .HasForeignKey(a => a.DisciplineId);

            modelBuilder.Entity<Cours>()
                .HasOne(c => c.User) // Propriété de navigation de type User
                .WithMany(user => user.CoursEnseignes) // Collection dans la classe User
                .HasForeignKey(c => c.UserId) // La clé étrangère (string)
                .OnDelete(DeleteBehavior.Restrict);
                }
    }
}
