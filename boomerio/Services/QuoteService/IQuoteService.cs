using boomerio.DTOs.QuoteDTOs;

namespace boomerio.Services.QuoteService
{
    public interface IQuoteService
    {
        Task<IEnumerable<QuoteDto>> GetAll();
        Task<QuoteDto?> GetById(int id);
        Task<IEnumerable<QuoteDto>> GetByCharacterId(int idCharacter);
        Task<QuoteDto?> GetRandomQuote();
        Task<IEnumerable<QuoteDto>> GetByQuery(string query);
        Task<QuoteDto> CreateQuote(QuoteCreationDto quote);
        Task<QuoteDto> UpdateQuoteValue(int quoteId, QuoteValueUpdateDto quote);
        Task<QuoteDto> UpdateCharacterIdValue(int quoteId, QuoteCharacterIdUpdateDto characterId);
        Task DeleteQuote(int id);
    }
}
