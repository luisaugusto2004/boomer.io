using boomer_shooter_api.DTOs.FranchiseDTOs;

namespace boomer_shooter_api.Services.FranchiseService
{
    public interface IFranchiseService
    {
        Task<List<FranchiseDto>> GetAllAsync();
        Task<FranchiseDto?> GetById(int id);
    }
}