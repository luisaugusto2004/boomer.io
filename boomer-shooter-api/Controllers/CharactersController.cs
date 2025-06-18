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
    }
}
