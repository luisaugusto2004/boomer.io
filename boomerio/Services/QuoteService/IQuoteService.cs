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
        Task<QuoteDto> Create(QuoteCreationDto quote);
    }
}
