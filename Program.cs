using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASPPorcelette.API.Data;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Services; 
using Microsoft.OpenApi.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Repository.Implementation;
using ASPPorcelette.API.MappingProfiles;
using ASPPorcelette.API.Services.Interfaces;
using ASPPorcelette.API.Services.Implementation;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Ajouté
using Microsoft.IdentityModel.Tokens; // Ajouté
using System.Text;
using ASPPorcelette.API.Seed;
using ASPPorcelette.API.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure; // Ajouté

System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("VueAppPolicy",
        policy =>
        {
            // 🎯 L'URL du FRONT-END : vérifiez le port de votre projet Vue (Vite = 5173 par défaut)
            policy.WithOrigins("http://localhost:5173") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// --- 1. CONFIGURATION DE LA BASE DE DONNÉES (DBContext) ---
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddScoped<ITokenService, TokenService>(); 

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' n'a pas été trouvée.");

// ApplicationDbContext gère à la fois vos modèles d'application ET les tables Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. CONFIGURATION D'ASP.NET IDENTITY (VERSION CORRIGÉE) ---

// On utilise UNIQUEMENT AddIdentity qui inclut les User Manager, Role Manager, etc.
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        // Configuration de la robustesse du mot de passe
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>() // Pointe UNIQUEMENT vers ApplicationDbContext
    .AddDefaultTokenProviders(); 

// --- 3. CONFIGURATION DES SERVICES (Vos couches Repository et Service) ---

// *********** INJECTIONS DE DÉPENDANCES ***********
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddScoped<ISenseiRepository, SenseiRepository>();
builder.Services.AddScoped<ISenseiService, SenseiService>();

builder.Services.AddScoped<IDisciplineRepository, DisciplineRepository>();
builder.Services.AddScoped<IDisciplineService, DisciplineService>();

builder.Services.AddScoped<IAdherentRepository, AdherentRepository>();
builder.Services.AddScoped<IAdherentService, AdherentService>();

builder.Services.AddScoped<ICoursRepository, CoursRepository>();
builder.Services.AddScoped<ICoursService, CoursService>();

builder.Services.AddScoped<IHoraireRepository, HoraireRepository>();
builder.Services.AddScoped<IHoraireService, HoraireService>();

builder.Services.AddScoped<IApprendreRepository, ApprendreRepository>();
builder.Services.AddScoped<IApprendreService, ApprendreService>();

builder.Services.AddScoped<ITypeEvenementRepository, TypeEvenementRepository>();
builder.Services.AddScoped<ITypeEvenementService, TypeEvenementService>();

builder.Services.AddScoped<IEvenementRepository, EvenementRepository>();
builder.Services.AddScoped<IEvenementService, EvenementService>();

builder.Services.AddScoped<IActualiteRepository, ActualiteRepository>();
builder.Services.AddScoped<IActualiteService, ActualiteService>();

builder.Services.AddScoped<ICategorieTransactionRepository, CategorieTransactionRepository>();
builder.Services.AddScoped<ICategorieTransactionService, CategorieTransactionService>();

builder.Services.AddScoped<ICompteRepository, CompteRepository>();
builder.Services.AddScoped<ICompteService, CompteService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<ITarifRepository, TarifRepository>();
builder.Services.AddScoped<ITarifService, TarifService>();

// Enregistrement des services d'authentification
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();






// Program.cs

// --- 4. CONFIGURATION DE L'API (Contrôleurs et Swagger) ---
builder.Services.AddControllers()
    // Cette configuration demande au sérialiseur d'ignorer les boucles.
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// 4. Configuration de l'Authentification JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Doit correspondre à la valeur dans appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], // Doit correspondre à la valeur dans appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)) // Doit correspondre à la clé secrète
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                return Task.CompletedTask;
            }
        };
    });
    
    


// Ajout des services Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ASPPorcelette API", Version = "v1" });

    // Définition du schéma de sécurité (JWT Bearer)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrez 'Bearer' suivi d'un espace et de votre jeton JWT.\n\nExemple : Bearer abc123xyz"
    });

    // Application du schéma à toutes les routes protégées
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



var app = builder.Build();



// --- 5. CONFIGURATION DU PIPELINE HTTP ---

// --- Exécution du Seeding des Rôles (Ajouté) ---
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    // Assurez-vous que la classe AuthDbContextSeed existe et contient la méthode SeedRolesAsync
    await AuthDbContextSeed.SeedRolesAsync(roleManager);
}
// --- Fin du Seeding ---


// Si l'environnement est en Développement, on active le Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPPorcelette API V1");
    });
}

app.MapGet("/debug/controllers", (IServiceProvider services) =>
{
    var actionDescriptorCollectionProvider = services.GetRequiredService<IActionDescriptorCollectionProvider>();
    var routes = actionDescriptorCollectionProvider.ActionDescriptors.Items
        .Select(x => new
        {
            Action = x.RouteValues["Action"],
            Controller = x.RouteValues["Controller"],
            Template = x.AttributeRouteInfo?.Template
        })
        .OrderBy(x => x.Controller)
        .ToList();
    
    return Results.Ok(routes);
});
// app.UseHttpsRedirection(); 

// Les services d'authentification et d'autorisation sont cruciaux pour une API sécurisée
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("VueAppPolicy");

// Mappe les requêtes HTTP aux méthodes de vos Controllers
app.MapControllers();

app.Run();
