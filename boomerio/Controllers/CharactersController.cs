using boomerio.DTOs;
using boomerio.DTOs.CharacterDTOs;
using boomerio.Services.CharacterService;
using boomerio.Services.FranchiseService;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        public readonly ICharacterService _characterService;
        public readonly IFranchiseService _franchiseService;

        public CharactersController(ICharacterService characterService, IFranchiseService franchiseService)
        {
            _characterService = characterService;
            _franchiseService = franchiseService;
        }

        /// <summary>
        /// Retrieves all characters.
        /// If no characters are available, it returns an empty collection.
        /// </summary>
        /// <response code="200">Returns a collection of characters.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(IEnumerable<CharacterDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetAllAsync()
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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
        /// Retrieves a collection of characters by their franchise ID.
        /// </summary>
        /// <param name="idFranchise">The ID of the franchise to which the characters belong</param>
        /// <response code="200">Retrieves all characters associated with a specific franchise ID.</response>
        /// <response code="400">If the franchise ID is not valid or cannot be parsed.</response>
        /// <response code="404">If the franchise with the specified ID does not exist.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(IEnumerable<CharacterDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("franchise/{idFranchise}")]
        public async Task<ActionResult<IEnumerable<CharacterDto>>> GetByFranchiseId(int idFranchise)
        {
            if (idFranchise <= 0)
            {
                return BadRequest(new ApiError("BadRequest", 400, "ID must be greater than zero."));
            }
            var franchiseExists = await _franchiseService.Exists(idFranchise);
            if (!franchiseExists)
            {
                return NotFound(
                    new ApiError(
                        "NotFound",
                        404,
                        $"Franchise not found for the id {idFranchise}."
                    )
                );
            }
            var characters = await _characterService.GetByFranchiseId(idFranchise);
            return Ok(characters);
        }
    }
}
