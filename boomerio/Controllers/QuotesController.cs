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

        [HttpGet("random")]
        public async Task<ActionResult<QuoteDto>> GetRandomQuote()
        {
            var quote = await _quoteService.GetRandomQuote();
            if (quote == null)
            {
                return NotFound(new
                {
                    type = "NotFound",
                    status = 404,
                    message = "No quotes available in the system."
                });
            }
            return Ok(quote);
        }

        [HttpGet]
        public async Task<ActionResult<List<QuoteDto>>> GetAll()
        {
            var quotes = await _quoteService.GetAll();
            return Ok(quotes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuoteDto>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    type = "BadRequest",
                    status = 400,
                    message = "ID must be greater than zero."
                });
            }
            var quote = await _quoteService.GetById(id);
            if (quote == null)
            {
                return NotFound(new
                {
                    type = "NotFound",
                    status = 404,
                    message = "Quote not found."
                });
            }
            return Ok(quote);
        }

        [HttpGet("character/{idCharacter}")]
        public async Task<ActionResult<List<QuoteDto>>> GetByCharacterId(int idCharacter)
        {
            if (idCharacter <= 0)
            {
                return BadRequest(new
                {
                    type = "BadRequest",
                    status = 400,
                    message = "Character ID must be greater than zero."
                });
            }
            var quotes = await _quoteService.GetByCharacterId(idCharacter);
            if (quotes == null || !quotes.Any())
            {
                return NotFound(new
                {
                    type = "NotFound",
                    status = 404,
                    message = "No quotes found for this character."
                });
            }
            return Ok(quotes);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<QuoteDto>>> GetByTerm([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new
                {
                    type = "BadRequest",
                    status = 400,
                    message = "Search term cannot be empty."
                });
            }
            var quotes = await _quoteService.GetByTerm(term);
            if (quotes == null || !quotes.Any())
            {
                return NotFound(new
                {
                    type = "NotFound",
                    status = 404,
                    message = "No quotes found for the given term."
                });
            }
            return Ok(quotes);
        }
    }
}
