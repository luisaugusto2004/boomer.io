using boomer_shooter_api.Models;

namespace boomer_shooter_api.Repositories.FranchiseRepository
{
    public interface IFranchiseRepository
    {
        Task<FranchiseModel?> GetById(int id);
        Task<List<FranchiseModel>> GetAll();
    }
}