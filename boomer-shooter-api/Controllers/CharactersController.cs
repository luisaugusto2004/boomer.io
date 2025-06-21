using boomer_shooter_api.DTOs.CharacterDTOs;
using boomer_shooter_api.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace boomer_shooter_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {

        public readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CharacterDto>>> GetAllAsync()
        {
            var characters = await _characterService.GetAllAsync();
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDto>> GetById(int id)
        {
            var character = await _characterService.GetById(id);
            if (character == null)
            {
                return NotFound(new
                {
                    type = "NotFound",
                    status = 404,
                    message = "Character not found."
                });
            }
            return Ok(character);
        }
    }
}
