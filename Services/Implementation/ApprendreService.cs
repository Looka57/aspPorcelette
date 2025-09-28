using ASPPorcelette.API.DTOs.Apprendre;
using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using AutoMapper;

public class ApprendreService : IApprendreService
{
    private readonly IApprendreRepository _apprendreRepository;
    private readonly IMapper _mapper;

    public ApprendreService(IApprendreRepository apprendreRepository, IMapper mapper)
    {
        _apprendreRepository = apprendreRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Apprendre>> GetAllInscriptionsAsync()
    {
        return await _apprendreRepository.GetAllWithRelationsAsync();
    }

    public async Task<Apprendre?> GetInscriptionByIdsAsync(int adherentId, int disciplineId)
    {
        return await _apprendreRepository.GetByIdsAsync(adherentId, disciplineId);
    }

    public Task<Apprendre?> GetInscriptionByIdAsync(int id)
    {
        throw new NotSupportedException("Utilisez GetInscriptionByIdsAsync car la clé est composite");
    }

    public async Task<Apprendre> CreateInscriptionAsync(ApprendreCreateDto inscriptionDto)
    {
        var apprendreEntity = _mapper.Map<Apprendre>(inscriptionDto);
        return await _apprendreRepository.AddAsync(apprendreEntity);
    }

    public async Task<bool> DeleteInscriptionAsync(int adherentId, int disciplineId)
    {
        return await _apprendreRepository.DeleteAsync(adherentId, disciplineId);
    }
}
