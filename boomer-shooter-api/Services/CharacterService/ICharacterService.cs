using boomer_shooter_api.DTOs.CharacterDTOs;

namespace boomer_shooter_api.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<List<CharacterDto>> GetAllAsync();
        Task<CharacterDto> GetById(int id);
    }
}
