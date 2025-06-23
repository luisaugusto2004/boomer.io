using boomerio.DTOs.QuoteDTOs;
using boomerio.Models;
using boomerio.Repositories.QuoteRepository;

namespace boomerio.Services.QuoteService
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public async Task<List<QuoteDto>> GetAll()
        {
            var quotes = await _quoteRepository.GetAllAsync();
            if (quotes == null)
            {
                return new List<QuoteDto>();
            }
            return quotes.Select(ToDto).ToList();
        }
        public async Task<List<QuoteDto>> GetByCharacterId(int idCharacter)
        {
            var quotes = await _quoteRepository.GetByCharacterId(idCharacter);

            if (quotes == null || !quotes.Any())
            {
                return new List<QuoteDto>();
            }
            return quotes.Select(ToDto).ToList();
        }

        public async Task<QuoteDto?> GetById(int id)
        {
            var quote = await _quoteRepository.GetByIdAsync(id);

            if (quote == null)
            {
                return null;
            }
            return ToDto(quote);
        }

        public async Task<QuoteDto?> GetRandomQuote()
        {
            var quote = await _quoteRepository.GetRandomQuote();

            if (quote == null)
            {
                return null;
            }
            return ToDto(quote);
        }

        public QuoteDto ToDto(QuoteModel quote) => new QuoteDto
        {
            Id = quote.Id,
            CreatedAt = quote.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
            UpdatedAt = quote.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
            Franchise = quote.Character.Franchise.Name,
            IconUrl = quote.Character.Franchise.IconUrl,
            Character = quote.Character.Name,
            Value = quote.QuoteText
        };
    }
}
