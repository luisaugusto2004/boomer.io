using boomerio.DTOs.FranchiseDTOs;

namespace boomerio.Services.FranchiseService
{
    public interface IFranchiseService
    {
        Task<List<FranchiseDto>> GetAllAsync();
        Task<FranchiseDto?> GetById(int id);
    }
}
