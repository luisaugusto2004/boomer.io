using boomerio.DTOs.CharacterDTOs;
using boomerio.Models;
using boomerio.Repositories.CharacterRepository;

namespace boomerio.Services.CharacterService
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
            return characters.Select(ToDto).ToList();
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

        public async Task<List<CharacterDto>> GetByFranchiseId(int idFranchise)
        {
            var characters = await _characterRepository.GetByFranchiseId(idFranchise);

            if (characters == null || !characters.Any())
            {
                return new List<CharacterDto>();
            }
            return characters.Select(ToDto).ToList();
        }

        public CharacterDto ToDto(CharacterModel character) => new CharacterDto
        {
            Id = character.Id,
            Franchise = character.Franchise.Name,
            Name = character.Name
        };
    }
}
