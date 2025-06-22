using boomerio.Repositories.FranchiseRepository;
using boomerio.DTOs.FranchiseDTOs;
using boomerio.Models;

namespace boomerio.Services.FranchiseService
{
    public class FranchiseService : IFranchiseService
    {
        private readonly IFranchiseRepository _franchiseRepository;

        public FranchiseService(IFranchiseRepository franchiseRepository)
        {
            _franchiseRepository = franchiseRepository;
        }

        public async Task<List<FranchiseDto>> GetAllAsync()
        {
            var franchises = await _franchiseRepository.GetAll();

            if (franchises == null || !franchises.Any())
            {
                return new List<FranchiseDto>();
            }
            return franchises.Select(ToDto).ToList();
        }

        public async Task<FranchiseDto?> GetById(int id)
        {
            var franchise = await _franchiseRepository.GetById(id);
            if (franchise == null)
            {
                return null;
            }
            return ToDto(franchise);
        }

        public FranchiseDto ToDto(FranchiseModel franchise) => new FranchiseDto
        {
            Id = franchise.Id,
            Name = franchise.Name,
        };
    }
}