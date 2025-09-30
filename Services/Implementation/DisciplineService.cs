using ASPPorcelette.API.Models;
using ASPPorcelette.API.Repository.Interfaces;
using ASPPorcelette.API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPPorcelette.API.Services.Implementation
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository _repository;

        public DisciplineService(IDisciplineRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Discipline>> GetAllDisciplinesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Discipline?> GetDisciplineByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Discipline> GetByIdAsync(int id)
        {
            var discipline = await _repository.GetByIdAsync(id);
            if (discipline == null)
            {
                throw new KeyNotFoundException($"Discipline with ID {id} not found.");
            }
            return discipline;
        }
    }
}
