using boomerio.Models;

namespace boomerio.Repositories.CharacterRepository
{
    public interface ICharacterRepository
    {
        Task<CharacterModel?> GetById(int id);
        Task<List<CharacterModel>> GetAll();
        Task<List<CharacterModel>> GetByFranchiseId(int franchiseId);
    }
}
