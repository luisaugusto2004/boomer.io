using boomer_shooter_api.DTOs.QuoteDTOs;
using boomer_shooter_api.Services.QuoteService;
using Microsoft.AspNetCore.Mvc;

namespace boomer_shooter_api.Controllers
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
    }
}
