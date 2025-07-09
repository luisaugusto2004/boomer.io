using boomerio.DTOs;
using boomerio.DTOs.CharacterDTOs;
using boomerio.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Controllers
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

        /// <summary>
        /// Retrieves all characters.
        /// If no characters are available, it returns an empty list.
        /// </summary>
        /// <response code="200">Returns a list of characters.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(List<CharacterDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<CharacterDto>>> GetAllAsync()
        {
            var characters = await _characterService.GetAllAsync();
            return Ok(characters);
        }

        /// <summary>
        /// Retrieves a character by its ID.
        /// </summary>
        /// <param name="id">The ID of a given character</param>
        /// <response code="200">Returns the character with the specified ID.</response>
        /// <response code="400">If the ID is less than or equal to zero.</response>
        /// <response code="404">If the character with the specified ID does not exist.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(CharacterDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDto>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiError("BadRequest", 400, "ID must be greater than zero."));
            }
            var character = await _characterService.GetById(id);
            if (character == null)
            {
                return NotFound(
                    new ApiError("NotFound", 404, $"Character not found for the id {id}.")
                );
            }
            return Ok(character);
        }

        /// <summary>
        /// Retrieves characters by their franchise ID.
        /// </summary>
        /// <param name="idFranchise">The ID of the franchise to which the characters belong</param>
        /// <response code="200">Returns the list of characters for the specified franchise ID.</response>
        /// <response code="400">If the ID is less than or equal to zero.</response>
        /// <response code="404">If the franchise with the specified ID does not exist or has no characters.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(List<CharacterDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
        [HttpGet("franchise/{idFranchise}")]
        public async Task<ActionResult<List<CharacterDto>>> GetByFranchiseId(int idFranchise)
        {
            if (idFranchise <= 0)
            {
                return BadRequest(new ApiError("BadRequest", 400, "ID must be greater than zero."));
            }
            var characters = await _characterService.GetByFranchiseId(idFranchise);
            if (characters == null || !characters.Any())
            {
                return NotFound(
                    new ApiError(
                        "NotFound",
                        404,
                        $"No characters found for franchise ID {idFranchise}."
                    )
                );
            }
            return Ok(characters);
        }
    }
}
