using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;

namespace ASPPorcelette.API.Services
{
    public class SenseiService : ISenseiService
    {
        private readonly ISenseiRepository _senseiRepository;

        public SenseiService(ISenseiRepository senseiRepository)
        {
            _senseiRepository = senseiRepository;
        }

        public async Task<IEnumerable<Sensei>> GetAllSenseisAsync()
        {
            return await _senseiRepository.GetAllAsync();
        }

        public async Task<Sensei?> GetSenseiByIdAsync(int id)
        {
            return await _senseiRepository.GetByIdAsync(id);
        }

        public async Task<Sensei> CreateSenseiAsync(Sensei sensei)
        {
            return await _senseiRepository.AddAsync(sensei);
        }

        public async Task<Sensei> UpdateSenseiAsync(Sensei sensei)
        {
            return await _senseiRepository.UpdateSensei(sensei);
        }

        public async Task<Sensei> DeleteSenseiAsync(int id)
        {
            return await _senseiRepository.DeleteSensei(id);
        }
    }
}