using boomer_shooter_api.DTOs.CharacterDTOs;
using boomer_shooter_api.DTOs.QuoteDTOs;
using boomer_shooter_api.Models;

namespace boomer_shooter_api.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ResponseModel<List<CharacterDto>>> GetAllAsync();
        Task<ResponseModel<CharacterDto>> GetById(int id);
    }
}
