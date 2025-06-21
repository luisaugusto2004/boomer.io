using boomer_shooter_api.DTOs.QuoteDTOs;

namespace boomer_shooter_api.Services.QuoteService
{
    public interface IQuoteService
    {
        Task<List<QuoteDto>> GetAll();
        Task<QuoteDto?> GetById(int id);
        Task<List<QuoteDto>> GetByCharacterId(int idCharacter);
        Task<QuoteDto?> GetRandomQuote();
    }
}
