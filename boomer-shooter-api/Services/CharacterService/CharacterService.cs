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

        public async Task<List<CharacterDto>> GetAllAsync()
        {
            var characters = await _characterRepository.GetAll();

            if (characters == null)
            {
                return new List<CharacterDto>();
            }

            var characterDtos = characters.Select(c => ToDto(c)).ToList();
            return characterDtos;
        }

        public async Task<CharacterDto?> GetById(int id)
        {
            var character = await _characterRepository.GetById(id);
            if (character == null)
            {
                return null;
            }

            return ToDto(character);
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
    }
}
