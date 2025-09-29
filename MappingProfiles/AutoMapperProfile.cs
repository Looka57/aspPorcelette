using AutoMapper;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Adherent;

namespace ASPPorcelette.API.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        // AutoMapper gère automatiquement les propriétés ayant le même nom.
        // Il sait aussi mapper la propriété 'DisciplinePrincipale' (qui est un objet) 
        // vers le 'DisciplinePrincipale' (qui est un DTO), car nous avons défini un mapping pour Discipline -> DisciplineDto.
        public AutoMapperProfile()
        {
            // ----------------------------------------------------
            // Mapping pour Discipline (Modèle vers DTO)     
            CreateMap<Discipline, DisciplineDto>();
            // ----------------------------------------------------

            // ----------------------------------------------------
            // Mapping pour Sensei (Modèle vers DTO)
            // ----------------------------------------------------
            CreateMap<Sensei, SenseiDto>();
            CreateMap<SenseiCreateDto, Sensei>();//(POST : du DTO de création au Modèle)
            CreateMap<SenseiUpdateDto, Sensei>();//(PUT/PATCH : du DTO de mise à jour au Modèle existant)
            CreateMap<Sensei, SenseiUpdateDto>();//(PATCH : du Modèle existant au DTO, pour initialiser le patch)


            // ----------------------------------------------------
            // Mappings pour la ressource Adherent (CRUD complet)
            // ----------------------------------------------------
            CreateMap<Adherent, AdherentDto>(); // 1. Entité vers DTO de sortie 
            CreateMap<Adherent, AdherentUpdateDto>();  // 2. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<AdherentCreateDto, Adherent>();  // 3. DTO de création vers Entité (POST)
            CreateMap<AdherentUpdateDto, Adherent>();// 4. DTO de mise à jour vers Entité (PUT/PATCH)

            // ----------------------------------------------------
            // Mappings pour la ressource Cours
            // ----------------------------------------------------
            CreateMap<Models.Cours, DTOs.Cours.CoursDto>();   // 1. Entité vers DTO de sortie (GET)
            CreateMap<DTOs.Cours.CoursCreateDto, Models.Cours>();   // 2. DTO de création vers Entité (POST)
            CreateMap<Models.Cours, DTOs.Cours.CoursUpdateDto>();   // 3. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<DTOs.Cours.CoursUpdateDto, Models.Cours>();  // 4. DTO de mise à jour vers Entité (PUT/PATCH)

            // / ----------------------------------------------------
            // Mappings pour la ressource Horaire
            // ----------------------------------------------------
            CreateMap<Models.Horaire, DTOs.Horaire.HoraireDto>();

            // / ----------------------------------------------------
            // Mappings pour la ressource Apprendre
            // ----------------------------------------------------
            CreateMap<Models.Apprendre, DTOs.Apprendre.ApprendreDto>();
            CreateMap<DTOs.Apprendre.ApprendreCreateDto, Models.Apprendre>();

            // / ----------------------------------------------------
            // Mappings pour la ressource TypeEvenement
            // ----------------------------------------------------
            CreateMap<TypeEvenement, DTOs.TypeEvenement.TypeEvenementDto>();
            CreateMap<DTOs.TypeEvenement.TypeEvenementCreateDto, TypeEvenement>();
            CreateMap<DTOs.TypeEvenement.TypeEvenementUpdateDto, TypeEvenement>();
            CreateMap<TypeEvenement, DTOs.TypeEvenement.TypeEvenementUpdateDto>();

        }
    }
}
