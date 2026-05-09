using boomerio.DTOs;
using boomerio.DTOs.QuoteDTOs;
using boomerio.Exceptions;
using boomerio.Models;
using boomerio.Repositories.QuoteRepository;
using boomerio.Services.CharacterService;
using System.Collections;

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

        public async Task<QuoteDto> CreateQuote(QuoteCreationDto quote)
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

        public async Task<QuoteDto> UpdateQuoteValue(int quoteId, QuoteValueUpdateDto quote)
        {
            if (quote == null || string.IsNullOrWhiteSpace(quote.Value) || quoteId <= 0 || quote.Value.Length < 5)
            {
                throw new BadRequestException("Quote value and a valid quote ID are required.");
            }
            var originalQuote = await _quoteRepository.GetByIdAsync(quoteId);
            if (originalQuote == null)
            {
                throw new NotFoundException("The quote with the given id was not found");
            }
            var editedQuote = originalQuote;
            editedQuote.QuoteText = quote.Value;
            await _quoteRepository.Update(editedQuote);
            return ToDto(editedQuote);
        }
        
        public async Task<QuoteDto> UpdateCharacterIdValue(int quoteId, QuoteCharacterIdUpdateDto characterId)
        {
            if (quoteId <= 0 || characterId.CharacterId <= 0)
            {
                throw new BadRequestException("A valid quote id and character id are required");
            }
            var quote = await _quoteRepository.GetByIdForUpdateAsync(quoteId);
            if (quote == null)
            {
                throw new NotFoundException("Quote does not exist");
            }
            if (!await _characterService.Exists(characterId.CharacterId))
            {
                throw new NotFoundException("Character does not exist");
            }
            if (quote.CharacterId == characterId.CharacterId)
            {
                throw new BadRequestException("This quote already belongs to this character");
            }
            quote.CharacterId = characterId.CharacterId;
            await _quoteRepository.Update(quote);
            var updatedQuote = await _quoteRepository.GetByIdForUpdateAsync(quoteId);
            if (updatedQuote == null)
                throw new NotFoundException("Quote not found after update");

            return ToDto(updatedQuote);
        }

        public async Task DeleteQuote(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("A valid quote ID is required.");
            }
            var quote = await _quoteRepository.GetByIdAsync(id);
            if (quote == null)
            {
                throw new NotFoundException("The quote with the given id was not found");
            }
            await _quoteRepository.Delete(quote);
        }

        public QuoteDto ToDto(QuoteModel quote) =>
            new QuoteDto
            {
                Id = quote.Id,
                CreatedAt = quote.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = quote.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Franchise = quote.Character?.Franchise?.Name,
                IconUrl = quote.Character?.Franchise?.IconUrl,
                CharacterId = quote.CharacterId,
                Character = quote.Character?.Name,
                Value = quote.QuoteText,
            };
    }
}
