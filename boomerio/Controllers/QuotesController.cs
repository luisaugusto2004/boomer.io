using boomerio.DTOs;
using boomerio.DTOs.QuoteDTOs;
using boomerio.Services.QuoteService;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        /// <summary>
        /// Retrieves a random quote from the database.
        /// If no quotes are available, it returns a 404 Not Found error.
        /// </summary>
        /// <response code="200">Returns a random quote.</response>
        /// <response code="404">If no quotes are available.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(QuoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("random")]
        public async Task<ActionResult<QuoteDto>> GetRandomQuote()
        {
            var quote = await _quoteService.GetRandomQuote();
            if (quote == null)
            {
                return NotFound(
                    new ApiError("NotFound", 404, "No quotes available at the moment.")
                );
            }
            return Ok(quote);
        }

        /// <summary>
        /// Retrieves all quotes from the database.
        /// If no quotes are available, it returns an empty list.
        /// </summary>
        /// <response code="200">Returns a list of quotes.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(List<QuoteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<QuoteDto>>> GetAll()
        {
            var quotes = await _quoteService.GetAll();
            return Ok(quotes);
        }

        /// <summary>
        /// Retrieves a quote by its ID.
        /// </summary>
        /// <param name="id">The ID of the quote to retrieve.</param>
        /// <response code="200">Returns the quote with the specified ID.</response>
        /// <response code="400">If the ID is less than or equal to zero.</response>
        /// <response code="404">If the quote with the specified ID does not exist.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(QuoteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteDto>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiError("BadRequest", 400, "ID must be greater than zero."));
            }
            var quote = await _quoteService.GetById(id);
            if (quote == null)
            {
                return NotFound(new ApiError("NotFound", 404, $"Quote not found for the id {id}."));
            }
            return Ok(quote);
        }

        /// <summary>
        /// Retrieves quotes by character ID.
        /// </summary>
        /// <param name="idCharacter">The ID of the character whose quotes are to be retrieved.</param>
        /// <response code="200">Returns a list of quotes for the specified character ID.</response>
        /// <response code="400">If the character ID is less than or equal to zero.</response>
        /// <response code="404">If no quotes are found for the specified character ID.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(List<QuoteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("character/{idCharacter}")]
        public async Task<ActionResult<List<QuoteDto>>> GetByCharacterId(int idCharacter)
        {
            if (idCharacter <= 0)
            {
                return BadRequest(
                    new ApiError("BadRequest", 400, "Character ID must be greater than zero.")
                );
            }
            var quotes = await _quoteService.GetByCharacterId(idCharacter);
            if (quotes == null || !quotes.Any())
            {
                return NotFound(
                    new ApiError(
                        "NotFound",
                        404,
                        $"No quotes found for character ID {idCharacter}."
                    )
                );
            }
            return Ok(quotes);
        }

        /// <summary>
        /// Retrieves quotes based on a search query.
        /// The query can be a substring of the quote text or character name.
        /// </summary>
        /// <param name="query">The search query to filter quotes.</param>
        /// <response code="200">Returns a list of quotes matching the search query.</response>
        /// <response code="400">If the query parameter is empty or whitespace.</response>
        /// <response code="404">If no quotes are found for the given query.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(List<QuoteDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("search")]
        public async Task<ActionResult<List<QuoteDto>>> GetByQuery([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(
                    new ApiError(
                        "BadRequest",
                        400,
                        "Query parameter cannot be empty or whitespace."
                    )
                );
            }
            var quotes = await _quoteService.GetByQuery(query);
            if (quotes == null || !quotes.Any())
            {
                return NotFound(
                    new ApiError("NotFound", 404, $"No quotes found for the query '{query}'.")
                );
            }
            return Ok(quotes);
        }
    }
}
