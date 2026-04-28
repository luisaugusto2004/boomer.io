using boomerio.DTOs;
using boomerio.DTOs.QuoteDTOs;
using boomerio.Exceptions;
using boomerio.Models;
using boomerio.Repositories.QuoteRepository;
using boomerio.Services.CharacterService;

namespace boomerio.Services.QuoteService
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly ICharacterService _characterService;

        public QuoteService(IQuoteRepository quoteRepository, ICharacterService characterService)
        {
            _quoteRepository = quoteRepository;
            _characterService = characterService;
        }

        public async Task<IEnumerable<QuoteDto>> GetAll()
        {
            var quotes = await _quoteRepository.GetAllAsync();
            return quotes.Select(ToDto).OrderBy(c => c.Character).ToList();
        }

        public async Task<IEnumerable<QuoteDto>> GetByCharacterId(int idCharacter)
        {
            var quotes = await _quoteRepository.GetByCharacterId(idCharacter);
            return quotes.Select(ToDto).ToList();
        }

        public async Task<IEnumerable<QuoteDto>> GetByQuery(string query)
        {
            var quotes = await _quoteRepository.GetByQueryAsync(query);
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

        public async Task<QuoteDto> Create(QuoteCreationDto quote)
        {
            if(quote == null || string.IsNullOrWhiteSpace(quote.Value) || quote.CharacterId <= 0)
            {
                throw new BadRequestException("Quote value and a valid character ID are required.");
            }
            if (!await _characterService.Exists(quote.CharacterId))
            {
                throw new NotFoundException("Character of given id was not found");
            }
            var quoteCreated = new QuoteModel
            {
                QuoteText = quote.Value,
                CharacterId = quote.CharacterId
            };
            var resultado = await _quoteRepository.Create(quoteCreated);

            return ToDto(resultado);
        }

        public QuoteDto ToDto(QuoteModel quote) =>
            new QuoteDto
            {
                Id = quote.Id,
                CreatedAt = quote.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = quote.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Franchise = quote.Character!.Franchise.Name,
                IconUrl = quote.Character.Franchise.IconUrl,
                CharacterId = quote.Character.Id,
                Character = quote.Character.Name,
                Value = quote.QuoteText,
            };
    }
}
