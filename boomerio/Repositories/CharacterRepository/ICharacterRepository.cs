using boomerio.Models;

namespace boomerio.Repositories.CharacterRepository
{
    public interface ICharacterRepository
    {
        Task<CharacterModel?> GetById(int id);
        Task<IEnumerable<CharacterModel>> GetAll();
        Task<IEnumerable<CharacterModel>> GetByFranchiseId(int franchiseId);
    }
}
