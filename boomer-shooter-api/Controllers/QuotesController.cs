using boomer_shooter_api.DTOs.QuoteDTOs;
using boomer_shooter_api.Models;
using boomer_shooter_api.Services.QuoteService;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<ResponseModel<QuoteDto>>> GetRandomQuote()
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
            return Ok(quote);
        }

        [HttpGet("character/{idCharacter}")]
        public async Task<ActionResult<List<QuoteDto>>> GetByCharacterId(int id)
        {
            var quotes = await _quoteService.GetByCharacterId(id);
            return Ok(quotes);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseModel<QuoteDto>>> CreateQuote(QuoteCreationDto dto)
        {
            var quote = await _quoteService.CreateQuote(dto);
            return Ok(quote);
        }

        [HttpPatch("PatchQuote/{id}")]
        public async Task<ActionResult<ResponseModel<QuoteDto>>> PatchQuote(int id, QuotePatchDto dto)
        {
            var quote = await _quoteService.PatchQuote(id, dto);
            return Ok(quote);
        }

        [HttpPatch("PatchCharacter/{idQuote}")]
        public async Task<ActionResult<ResponseModel<QuoteDto>>> PatchCharacter(int idQuote, PatchCharacterDto dto)
        {
            var quote = await _quoteService.PatchCharacter(idQuote, dto);
            return Ok(quote);
        }

        [HttpDelete("DeleteQuote/{id}")]
        public async Task<ActionResult<ResponseModel<QuoteDto>>> DeleteQuote(int id)
        {
            var quote = await _quoteService.DeleteQuote(id);
            return Ok(quote);
        }
    }
}
