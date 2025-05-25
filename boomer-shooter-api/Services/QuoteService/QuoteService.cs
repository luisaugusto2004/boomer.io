using boomer_shooter_api.DTOs.QuoteDTOs;
using boomer_shooter_api.Models;
using boomer_shooter_api.Repositories.CharacterRepository;
using boomer_shooter_api.Repositories.QuoteRepository;

namespace boomer_shooter_api.Services.QuoteService
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly ICharacterRepository _characterRepository;

        public QuoteService(IQuoteRepository quoteRepository, ICharacterRepository characterRepository)
        {
            _quoteRepository = quoteRepository;
            _characterRepository = characterRepository;
        }

        public async Task<ResponseModel<QuoteDto>> CreateQuote(QuoteCreationDto dto)
        {
            ResponseModel<QuoteDto> response = new ResponseModel<QuoteDto>();

            try
            {
                if (string.IsNullOrWhiteSpace(dto.Quote))
                {
                    response.Data = null;
                    response.Message = "The quote field must have something";
                    return response;
                }

                var character = await _characterRepository.FindCharacterAsync(dto.CharacterId);

                if (character == null)
                {
                    response.Data = null;
                    response.Message = "A character with this ID doesn't exist";
                    return response;
                }

                var quoteEntity = new QuoteModel
                {
                    QuoteText = dto.Quote,
                    Character = character
                };
                await _quoteRepository.AddAsync(quoteEntity);
                await _quoteRepository.SaveChangesAsync();

                var quote = new QuoteDto
                {
                    Id = quoteEntity.Id,
                    Franchise = character.Franchise.Name,
                    Character = character.Name,
                    Value = dto.Quote
                };

                response.Data = quote;
                response.Message = "Quote created with success!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<QuoteDto>> DeleteQuote(int id)
        {
            ResponseModel<QuoteDto> response = new ResponseModel<QuoteDto>();
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);

                if (quote == null)
                {
                    response.Data = null;
                    response.Message = "There is no quote with that ID";
                    return response;
                }
                _quoteRepository.Delete(quote);
                await _quoteRepository.SaveChangesAsync();
                response.Data = new QuoteDto
                {
                    Id = quote.Id,
                    Franchise = quote.Character.Franchise.Name,
                    Character = quote.Character.Name,
                    Value = quote.QuoteText
                };
                response.Message = "Quote was deleted with success!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<QuoteDto>>> GetAll()
        {
            ResponseModel<List<QuoteDto>> response = new ResponseModel<List<QuoteDto>>();
            try
            {
                var quotes = await _quoteRepository.GetAllAsync();

                if (quotes == null)
                {
                    response.Data = new List<QuoteDto>();
                    response.Message = "No quotes were found";
                    return response;
                }

                response.Data = quotes.Select(q => new QuoteDto
                {
                    Id = q.Id,
                    Franchise = q.Character.Franchise.Name,
                    Character = q.Character.Name,
                    Value = q.QuoteText
                }).ToList();
                response.Message = "All quotes returned!";
                return response;

            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<QuoteDto>>> GetByCharacterId(int id)
        {
            ResponseModel<List<QuoteDto>> response = new ResponseModel<List<QuoteDto>>();
            try
            {
                var quotes = await _quoteRepository.GetByCharacterId(id);

                if (quotes == null || !quotes.Any())
                {
                    response.Data = new List<QuoteDto>();
                    response.Message = "There is no character with that ID";
                    return response;
                }

                response.Data = quotes.Select(q => new QuoteDto
                {
                    Id = q.Id,
                    Value = q.QuoteText,
                    Character = q.Character.Name,
                    Franchise = q.Character.Franchise.Name
                }).ToList();
                response.Message = "All quotes returned!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<QuoteDto>> GetById(int id)
        {
            ResponseModel<QuoteDto> response = new ResponseModel<QuoteDto>();
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);

                if (quote == null)
                {
                    response.Data = null;
                    response.Message = "There is no quote with that ID";
                    return response;
                }

                response.Data = new QuoteDto
                {
                    Id = quote.Id,
                    Franchise = quote.Character.Franchise.Name,
                    Character = quote.Character.Name,
                    Value = quote.QuoteText
                };
                response.Message = "Fresh quote coming up!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<QuoteDto>> GetRandomQuote()
        {
            ResponseModel<QuoteDto> response = new ResponseModel<QuoteDto>();
            try
            {
                var quote = await _quoteRepository.GetRandomQuote();

                if (quote == null)
                {
                    response.Data = null;
                    response.Message = "There is no quotes in the database";
                    return response;
                }
                response.Data = ToDto(quote);
                response.Message = "A fresh random quote coming up!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<QuoteDto>> PatchCharacter(int idQuote, PatchCharacterDto dto)
        {
            ResponseModel<QuoteDto> response = new ResponseModel<QuoteDto>();
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(idQuote);

                if (quote == null)
                {
                    response.Data = null;
                    response.Message = "There is no quote with that ID";
                    return response;
                }

                var newCharacter = await _characterRepository.FindCharacterAsync(dto.NewCharacterId);
                if(newCharacter == null)
                {
                    response.Data = null;
                    response.Message = "There is no character with that ID";
                    return response;
                }

                quote.Character = newCharacter;

                await _quoteRepository.SaveChangesAsync();

                response.Data = new QuoteDto
                {
                    Id = quote.Id,
                    Franchise = quote.Character.Franchise.Name,
                    Character = quote.Character.Name,
                    Value = quote.QuoteText
                };
                response.Message = "Character patched with success!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<QuoteDto>> PatchQuote(int id, QuotePatchDto dto)
        {
            ResponseModel<QuoteDto> response = new ResponseModel<QuoteDto>();
            try
            {
                var quote = await _quoteRepository.GetByIdAsync(id);

                if (quote == null)
                {
                    response.Data = null;
                    response.Message = "There is no quote with that ID";
                    return response;
                }

                if (!string.IsNullOrEmpty(dto.Quote))
                    quote.QuoteText = dto.Quote;

                await _quoteRepository.SaveChangesAsync();

                response.Data = new QuoteDto
                {
                    Id = quote.Id,
                    Franchise = quote.Character.Franchise.Name,
                    Character = quote.Character.Name,
                    Value = quote.QuoteText
                };
                response.Message = "Quote patched with success!";
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.Message = $"An unexpected error occurred: {e.Message}";
                response.Status = false;
                return response;
            }
        }

        public QuoteDto ToDto(QuoteModel quote) => new QuoteDto
        {
            Id = quote.Id,
            Franchise = quote.Character.Franchise.Name,
            Character = quote.Character.Name,
            Value = quote.QuoteText
        };
    }
}
