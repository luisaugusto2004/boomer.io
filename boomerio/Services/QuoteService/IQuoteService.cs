using boomerio.DTOs.QuoteDTOs;

namespace boomerio.Services.QuoteService
{
    public interface IQuoteService
    {
        Task<List<QuoteDto>> GetAll();
        Task<QuoteDto?> GetById(int id);
        Task<List<QuoteDto>> GetByCharacterId(int idCharacter);
        Task<QuoteDto?> GetRandomQuote();
        Task<List<QuoteDto>> GetByTerm(string query);
    }
}
