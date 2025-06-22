using boomerio.Models;

namespace boomerio.Repositories.FranchiseRepository
{
    public interface IFranchiseRepository
    {
        Task<FranchiseModel?> GetById(int id);
        Task<List<FranchiseModel>> GetAll();
    }
}