using boomer_shooter_api.DTOs.CharacterDTOs;
using boomer_shooter_api.Models;
using boomer_shooter_api.Repositories.CharacterRepository;

namespace boomer_shooter_api.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        public readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<ResponseModel<List<CharacterDto>>> GetAllAsync()
        {
            ResponseModel<List<CharacterDto>> response = new ResponseModel<List<CharacterDto>>();
            try
            {
                var characters = await _characterRepository.GetAll();

                if(characters == null)
                {
                    response.Data = new List<CharacterDto>();
                    response.Message = "No characters were found";
                    return response;
                }
                response.Data = characters.Select(c => new CharacterDto
                {
                    Id = c.Id,
                    Franchise = c.Franchise.Name,
                    Name = c.Name
                    
                }).ToList();
                response.Message = "All characters returned!";

                return response;
                
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public Task<ResponseModel<CharacterDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public CharacterDto ToDto(CharacterModel character)
        {
            return new CharacterDto
            {
                Id = character.Id,
                Franchise = character.Franchise.Name,
                Name = character.Name
            };
        }

        public ResponseModel<CharacterDto> ExceptionResponse(Exception e)
        {
            return new ResponseModel<CharacterDto>
            {
                Data = null,
                Message = $"An unexpected error occurred: {e.Message}",
                Status = false
            };
        }
    }
}
