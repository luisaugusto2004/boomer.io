using boomer_shooter_api.DTOs.QuoteDTOs;
using boomer_shooter_api.Models;

namespace boomer_shooter_api.Services.QuoteService
{
    public interface IQuoteService
    {
        Task<ResponseModel<List<QuoteDto>>> GetAll();
        Task<ResponseModel<QuoteDto>> GetById(int id);
        Task<ResponseModel<List<QuoteDto>>> GetByCharacterId(int id);
        Task<ResponseModel<QuoteDto>> GetRandomQuote();
        Task<ResponseModel<QuoteDto>> CreateQuote(QuoteCreationDto dto);
        Task<ResponseModel<QuoteDto>> PatchQuote(int id, QuotePatchDto dto);
        Task<ResponseModel<QuoteDto>> PatchCharacter(int idQuote, PatchCharacterDto dto);
        Task<ResponseModel<QuoteDto>> DeleteQuote(int id);
    }
}
