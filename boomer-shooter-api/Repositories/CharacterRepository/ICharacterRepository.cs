using boomer_shooter_api.Models;

namespace boomer_shooter_api.Repositories.CharacterRepository
{
    public interface ICharacterRepository
    {
        Task<CharacterModel?> GetById(int id);
        Task<List<CharacterModel>> GetAll();
        Task<List<CharacterModel>> GetByFranchiseId(int franchiseId);
    }
}
