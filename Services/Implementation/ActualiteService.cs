using ASPPorcelette.API.Data;
using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class ActualiteService : IActualiteService
    {
        private readonly IActualiteRepository _actualiteRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public ActualiteService(IActualiteRepository actualiteRepository, IMapper mapper, UserManager<User> userManager, ApplicationDbContext context)
        {
            _actualiteRepository = actualiteRepository;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        // -----------------------------------------------------------------
        // Lecture (READ)
        // -----------------------------------------------------------------
        public async Task<IEnumerable<Actualite>> GetAllAsync()
        {
            return await _context.Actualites
                .Include(a => a.User)
                .Include(a => a.EvenementAssocie) 
                .OrderByDescending(a => a.DateDePublication)
                .ToListAsync();
        }

        public async Task<Actualite?> GetByIdAsync(int id)
        {
            return await _context.Actualites
                .Include(a => a.User)
                .Include(a => a.EvenementAssocie) 
                .FirstOrDefaultAsync(a => a.ActualiteId == id);
        }

        // -----------------------------------------------------------------
        // Création (CREATE)
        // -----------------------------------------------------------------

        public async Task<Actualite> CreateAsync(ActualiteCreateDto createDto)
        {
            // On récupère l'utilisateur (Sensei) via le UserId
            var user = await _userManager.FindByIdAsync(createDto.UserId);
            if (user == null)
            {
                throw new Exception("Utilisateur introuvable");
            }
            // ************************************************************
            // 🎯 VÉRIFICATION CRITIQUE DE LA CLÉ ÉTRANGÈRE (EvenementId)
            // ************************************************************
            int? finalEvenementId = createDto.EvenementId;

            if (finalEvenementId.HasValue)
            {
                // Vérifie si l'ID d'événement existe dans la base de données
                var evenementExists = await _context.Evenements.AnyAsync(e => e.EvenementId == finalEvenementId.Value);

                if (!evenementExists)
                {
                    // Si l'événement n'existe pas (la clé étrangère échouerait), 
                    // on force l'ID à NULL pour permettre la sauvegarde de l'article.
                    finalEvenementId = null;
                }
            }

            var actualite = new Actualite
            {
                Titre = createDto.Titre,
                Contenu = createDto.Contenu,
                ImageUrl = createDto.ImageUrl,
                UserId = user.Id,
                DateDePublication = DateTime.UtcNow,
                EvenementId = finalEvenementId,

            };
            Console.WriteLine($"[SERVICE] EvenementId dans CreateAsync : {createDto.EvenementId}");

            _context.Actualites.Add(actualite);
            await _context.SaveChangesAsync();

            return actualite;
        }

        // -----------------------------------------------------------------
        // Mise à Jour (UPDATE - PUT)
        // -----------------------------------------------------------------
        public async Task<bool> UpdateAsync(int id, ActualiteUpdateDto updateDto, string webRootPath)
{
    // 1️⃣ Récupérer l'article existant
    var existingActualite = await _context.Actualites.AsTracking()
        .FirstOrDefaultAsync(a => a.ActualiteId == id);

    if (existingActualite == null) 
        return false; // Article introuvable

    string oldImageUrl = existingActualite.ImageUrl;
    string newImageUrl = oldImageUrl; // Par défaut, on garde l'ancienne image

    // -----------------------------------------------------
    // 2️⃣ Gestion de l'image
    // -----------------------------------------------------

    if (updateDto.ImageFile != null)
    {
        DeletePhysicalFile(oldImageUrl, webRootPath);
        newImageUrl = await SavePhysicalFileAsync(updateDto.ImageFile, webRootPath);
    }
    else if (updateDto.DeleteExistingImage)
    {
        DeletePhysicalFile(oldImageUrl, webRootPath);
        newImageUrl = null;
    }

    // -----------------------------------------------------
    // 3️⃣ Vérification de la clé étrangère EvenementId
    // -----------------------------------------------------
    int? finalEvenementId = updateDto.EvenementId;

    if (finalEvenementId.HasValue && finalEvenementId.Value > 0)
    {
        var evenementExists = await _context.Evenements
            .AnyAsync(e => e.EvenementId == finalEvenementId.Value);

        if (!evenementExists)
        {
            finalEvenementId = null; // ID invalide → on met à null
        }
    }

    // -----------------------------------------------------
    // 4️⃣ Mapping des champs depuis le DTO
    // -----------------------------------------------------
    _mapper.Map(updateDto, existingActualite);

    // Champs spécifiques non mappés automatiquement
    existingActualite.ImageUrl = newImageUrl;
    existingActualite.EvenementId = finalEvenementId;
    existingActualite.DateDePublication = updateDto.DatePublication; // DateTime, pas string

    // -----------------------------------------------------
    // 5️⃣ Sauvegarde des changements
    // -----------------------------------------------------
    try
    {
        return await _context.SaveChangesAsync() > 0;
    }
    catch (DbUpdateConcurrencyException)
    {
        // Gérer la concurrence si nécessaire
        return false;
    }
}


        // Méthode pour sauvegarder le fichier sur le disque (réutilisée de la création)
        private async Task<string> SavePhysicalFileAsync(IFormFile imageFile, string webRootPath)
        {
            var uploadFolder = Path.Combine(webRootPath, "images", "actualites");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/actualites/{uniqueFileName}";
        }

        // Méthode pour supprimer le fichier physique
        private void DeletePhysicalFile(string? imageUrl, string webRootPath)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            // Attention : retire les '/' initiaux pour Path.Combine
            var relativePath = imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var imagePath = Path.Combine(webRootPath, relativePath);

            if (System.IO.File.Exists(imagePath))
            {
                try
                {
                    System.IO.File.Delete(imagePath);
                    Console.WriteLine($"✅ Image supprimée : {imagePath}");
                }
                catch (Exception ex)
                {
                    // Logguer l'erreur : la suppression du fichier est facultative, ne doit pas empêcher l'update de la DB
                    Console.WriteLine($"⚠️ Erreur suppression image : {ex.Message}");
                }
            }
        }
        // -----------------------------------------------------------------
        // Mise à Jour Partielle (UPDATE - PATCH)
        // -----------------------------------------------------------------

        public async Task<(Actualite? Actualite, bool Success)> PartialUpdateAsync(
            int id,
            JsonPatchDocument<ActualiteUpdateDto> patchDocument
        )
        {
            var actualiteEntity = await _actualiteRepository.GetByIdAsync(id);
            if (actualiteEntity == null)
            {
                return (null, false);
            }

            // 1. Mappage de l'entité vers le DTO de mise à jour pour appliquer le patch
            var actualiteDtoToPatch = _mapper.Map<ActualiteUpdateDto>(actualiteEntity);
            patchDocument.ApplyTo(actualiteDtoToPatch);

            // 2. Mappage inverse du DTO patché vers l'entité existante
            _mapper.Map(actualiteDtoToPatch, actualiteEntity);

            // 3. Mise à jour dans la base de données
            var success = await _actualiteRepository.UpdateAsync(actualiteEntity);

            if (success)
            {
                // Recharger l'entité avec les relations pour le retour au client
                var updatedActualite = await _actualiteRepository.GetByIdWithDetailsAsync(id);
                return (updatedActualite, true);
            }

            return (null, false);
        }

        // -----------------------------------------------------------------
        // Suppression (DELETE)
        // -----------------------------------------------------------------

        public async Task<bool> DeleteAsync(int id, string webRootPath)
        {
            // 1️⃣ Récupérer l'article pour avoir ImageUrl
            var actualite = await _actualiteRepository.GetByIdWithDetailsAsync(id);
            if (actualite == null) return false;

            // 2️⃣ Supprimer l'image physique si elle existe
            if (!string.IsNullOrEmpty(actualite.ImageUrl))
            {
                // Attention : retire les '/' initiaux pour Path.Combine
                var relativePath = actualite.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
                var imagePath = Path.Combine(webRootPath, relativePath);

                if (System.IO.File.Exists(imagePath))
                {
                    try
                    {
                        System.IO.File.Delete(imagePath);
                        Console.WriteLine($"✅ Image supprimée : {imagePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Erreur suppression image : {ex.Message}");
                    }
                }
            }

            // 3️⃣ Supprimer l'article en base
            return await _actualiteRepository.DeleteAsync(id);
        }

    }
}
