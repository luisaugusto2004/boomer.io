using boomerio.DTOs.CharacterDTOs;

namespace boomerio.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<IEnumerable<CharacterDto>> GetAllAsync();
        Task<CharacterDto?> GetById(int id);
        Task<IEnumerable<CharacterDto>> GetByFranchiseId(int idFranchise);
        Task<bool> Exists(int CharacterId);
    }
}
