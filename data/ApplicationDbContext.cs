using Microsoft.EntityFrameworkCore;
// using ASPPorcelette.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ASPPorcelette.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>

    {
        // Le constructeur est obligatoire pour passer les options à DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // public DbSet<YourModel> YourModels { get; set; }
    }
}

