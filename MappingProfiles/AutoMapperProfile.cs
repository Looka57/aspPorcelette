using AutoMapper;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.DTOs;
// using ASPPorcelette.API.DTOs.Sensei;
using ASPPorcelette.API.DTOs.Discipline;
using ASPPorcelette.API.DTOs.Adherent;
using ASPPorcelette.API.DTOs.Evenement;
using ASPPorcelette.API.DTOs.Actualite;
using ASPPorcelette.API.DTOs.CategorieTransaction;
using ASPPorcelette.API.DTOs.Compte;
using ASPPorcelette.API.DTOs.Transaction;
using ASPPorcelette.API.DTOs.Tarif;
using ASPPorcelette.API.Models.Identity;
using ASPPorcelette.API.DTOs.Cours;
using ASPPorcelette.API.DTOs.Horaire;

namespace ASPPorcelette.API.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ----------------------------------------------------
            // Mapping pour Discipline (Modèle vers DTO)     
            CreateMap<Discipline, DisciplineDto>()
            .ForMember(dest => dest.Cours, opt => opt.MapFrom(src => src.CoursAssocies));
            CreateMap<Cours, CoursDto>();
            CreateMap<Horaire, HoraireDto>();
            CreateMap<User, UserDto>();

            // ----------------------------------------------------

            // ----------------------------------------------------
            // Mapping pour User (Modèle vers DTO)
            // ----------------------------------------------------
            CreateMap<User, UserDto>()
                // Assure le mappage des propriétés avec les mêmes noms (Nom, Prenom, Email, etc.)
                // ET s'assure que la propriété PhotoUrl est incluse, même si elle n'est pas chargée par défaut.
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.PhotoUrl))
                .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Nom))
                .ForMember(dest => dest.Prenom, opt => opt.MapFrom(src => src.Prenom));

            // Mappage de l'entité User (source) vers le DTO minimaliste (destination)
            CreateMap<User, DTOs.Sensei.SenseiMinimalDto>();


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
            CreateMap<Models.Cours, DTOs.Cours.CoursDto>()
            .ForMember(
                        dest => dest.Sensei,                  // La propriété du DTO (Destination)
                        opt => opt.MapFrom(src => src.User)   // Mappe depuis la propriété de l'Entité (Source)
                        );  // 1. Entité vers DTO de sortie (GET)
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

            // / ----------------------------------------------------
            // Mappings pour la ressource Evenements
            // ----------------------------------------------------
            CreateMap<Evenement, EvenementDto>();  // AutoMapper mappera automatiquement TypeEvenement à TypeEvenementDto
            CreateMap<Evenement, EvenementUpdateDto>();  // 2. Entité vers DTO de mise à jour (pour l'initialisation du PATCH)
            CreateMap<EvenementCreateDto, Evenement>();  // 3. DTO de création vers Entité (POST)
            CreateMap<EvenementUpdateDto, Evenement>();// 4. DTO de mise à jour vers Entité (PUT/PATCH)

            // / ----------------------------------------------------
            // Mappings pour la ressource Actualite
            // ----------------------------------------------------
            CreateMap<Actualite, ActualiteDto>();  // AutoMapper mappera automatiquement Sensei à SenseiDto et Discipline à DisciplineDto
            CreateMap<Actualite, ActualiteUpdateDto>();  // 2. Entité
            CreateMap<ActualiteCreateDto, Actualite>();  // 3. DTO de création vers Entité (POST
            CreateMap<ActualiteUpdateDto, Actualite>()
                .ForMember(dest => dest.ActualiteId, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());


            // / ----------------------------------------------------
            // Mappings pour la ressource CategorieTransaction
            // ----------------------------------------------------
            CreateMap<CategorieTransaction, CategorieTransactionDto>();
            CreateMap<CategorieTransactionCreateDto, CategorieTransaction>();
            CreateMap<CategorieTransactionUpdateDto, CategorieTransaction>();
            CreateMap<CategorieTransaction, CategorieTransactionUpdateDto>();

            // / ----------------------------------------------------
            // Mappings pour la ressource Compte
            // ----------------------------------------------------
            CreateMap<Compte, CompteDto>();  // AutoMapper mappera automatiquement les Transactions à TransactionDto
            CreateMap<CompteCreateDto, Compte>();  // 2. DTO de création
            CreateMap<CompteUpdateDto, Compte>();// 3. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<Compte, CompteUpdateDto>();// 4. Entité vers DTO

            // / ----------------------------------------------------
            // Mappings pour la ressource Transaction
            // ----------------------------------------------------
            CreateMap<Transaction, TransactionDto>();  // AutoMapper mappera automatiquement Compte à CompteDto, CategorieTransaction à CategorieTransactionDto et Discipline à DisciplineDto
            CreateMap<TransactionCreateDto, Transaction>();  // 2. DTO de création vers Ent
            CreateMap<TransactionUpdateDto, Transaction>();// 3. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<Transaction, TransactionUpdateDto>();// 4. Entité vers DTO de mise

            // / ----------------------------------------------------
            // Mappings pour la ressource Tarif
            // ----------------------------------------------------
            CreateMap<Tarif, TarifDto>();  // AutoMapper mappera automatiquement Discipline à DisciplineDto
            CreateMap<TarifCreateDto, Tarif>();  // 2. DTO de création
            CreateMap<TarifUpdateDto, Tarif>();// 3. DTO de mise à jour vers Entité (PUT/PATCH)
            CreateMap<Tarif, TarifUpdateDto>();// 4. Entité vers DTO
        }
    }
}
