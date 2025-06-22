using boomerio.DTOs.CharacterDTOs;

namespace boomerio.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<CharacterDto>> GetAllAsync();
        Task<CharacterDto?> GetById(int id);
        Task<List<CharacterDto>> GetByFranchiseId(int idFranchise);
    }
}
