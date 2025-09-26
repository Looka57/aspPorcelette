using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services; 
using Microsoft.OpenApi.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Repository.Implementation;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION DE LA BASE DE DONNÉES (DBContext) ---

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' n'a pas été trouvée.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. CONFIGURATION D'ASP.NET IDENTITY ---

// !! CORRECTION IMPORTANTE !! Utiliser ApplicationUser au lieu de User
builder.Services.AddIdentity<User, IdentityRole>(options => 
    {
        // Options de sécurité (à affiner plus tard)
        options.SignIn.RequireConfirmedAccount = false; 
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() 
    .AddDefaultTokenProviders();

// --- 3. CONFIGURATION DES SERVICES (Vos couches Repository et Service) ---

// *********** INJECTIONS DE DÉPENDANCES ***********
builder.Services.AddScoped<ISenseiRepository, SenseiRepository>();
builder.Services.AddScoped<ISenseiService, SenseiService>();
// Si vous utilisez un modèle avec le dossier 'Repository.Interfaces' etc., ajustez les 'using' ci-dessus.

// Program.cs

// --- 4. CONFIGURATION DE L'API (Contrôleurs et Swagger) ---
builder.Services.AddControllers()
    // Cette configuration demande au sérialiseur d'ignorer les boucles.
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// ...


// Ajout des services Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// --- 5. CONFIGURATION DU PIPELINE HTTP ---

// Si l'environnement est en Développement, on active le Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPPorcelette API V1");
    });
}

// app.UseHttpsRedirection(); // Cette ligne est souvent commentée en dev pour simplifier les tests localhost

// Les services d'authentification et d'autorisation sont cruciaux pour une API sécurisée
app.UseAuthentication();
app.UseAuthorization();

// Mappe les requêtes HTTP aux méthodes de vos Controllers
app.MapControllers();

app.Run();
