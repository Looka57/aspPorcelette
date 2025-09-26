using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASPPorcelette.API.Data;   // Assurez-vous d'utiliser le bon namespace
using ASPPorcelette.API.Models; // Assurez-vous d'utiliser le bon namespace

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION DE LA BASE DE DONNÉES (DBContext) ---

// Récupère la chaîne de connexion du fichier appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' n'a pas été trouvée.");

// Enregistre votre DbContext avec SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. CONFIGURATION D'ASP.NET IDENTITY ---

// Configure le système d'identité (utilisateurs et rôles)
builder.Services.AddIdentity<User, IdentityRole>(options => 
    {
        // Options de sécurité (à affiner plus tard)
        options.SignIn.RequireConfirmedAccount = false; 
    })
    // Utilise votre ApplicationDbContext pour stocker les données d'identité
    .AddEntityFrameworkStores<ApplicationDbContext>() 
    .AddDefaultTokenProviders();

// --- 3. CONFIGURATION DES SERVICES (Vos couches Repository et Service) ---

// *********** PLACEZ VOS INJECTIONS DE DÉPENDANCES ICI ***********
// Ces lignes seront ajoutées au fur et à mesure que vous créerez vos interfaces et classes.
// Exemple :
// builder.Services.AddScoped<IAdherentRepository, AdherentRepository>();
// builder.Services.AddScoped<IAdherentService, AdherentService>();

// --- 4. CONFIGURATION DE L'API (Contrôleurs et Swagger) ---

// Ajoute le support des Contrôleurs (API REST)
builder.Services.AddControllers();

var app = builder.Build();

// --- 5. CONFIGURATION DU PIPELINE HTTP ---

// Configure le pipeline uniquement pour l'environnement de développement


app.UseHttpsRedirection();

// Les services d'authentification et d'autorisation sont cruciaux pour une API sécurisée
app.UseAuthentication();
app.UseAuthorization();

// Mappe les requêtes HTTP aux méthodes de vos Controllers
app.MapControllers();

app.Run();