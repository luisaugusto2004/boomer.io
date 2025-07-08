using boomerio.Controllers;
using boomerio.DTOs.QuoteDTOs;
using boomerio.Services.QuoteService;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Tests
{
    public class QuotesControllerTests
    {
        [Fact]
        public async Task GetRandomQuote_ReturnsExpectedQuote()
        {
            //Arange
            QuoteDto? expected = new QuoteDto
            {
                Value = "Quote test",
                Character = "Character test",
            };
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetRandomQuote())
                .Returns(Task.FromResult<QuoteDto?>(expected));

            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetRandomQuote();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<QuoteDto?>(okResult.Value);
            returned!.Value.Should().Be("Quote test");
            returned!.Character.Should().Be("Character test");
        }

        [Fact]
        public async Task GetRandomQuote_ReturnsNotFound_WhenNoQuote()
        {
            //Arange
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetRandomQuote()).Returns(Task.FromResult<QuoteDto?>(null));

            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetRandomQuote();

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetRandomQuote_ReturnsDifferentQuotes_OnMutipleCalls()
        {
            //Arrange
            var quote1 = new QuoteDto { Value = "Quote 1", Character = "Character 1" };
            var quote2 = new QuoteDto { Value = "Quote 2", Character = "Character 2" };
            var fakeService = A.Fake<IQuoteService>();

            A.CallTo(() => fakeService.GetRandomQuote())
                .ReturnsNextFromSequence(
                    Task.FromResult<QuoteDto?>(quote1),
                    Task.FromResult<QuoteDto?>(quote2)
                );
            var controller = new QuotesController(fakeService);

            //Act
            var result1 = await controller.GetRandomQuote();
            var result2 = await controller.GetRandomQuote();

            var quoteResult1 = Assert.IsType<OkObjectResult>(result1.Result);
            var quoteResult2 = Assert.IsType<OkObjectResult>(result2.Result);

            var returnedQuote1 = Assert.IsType<QuoteDto>(quoteResult1.Value);
            var returnedQuote2 = Assert.IsType<QuoteDto>(quoteResult2.Value);

            //Assert
            returnedQuote1!.Value.Should().NotBe(returnedQuote2.Value);
            returnedQuote1!.Character.Should().NotBe(returnedQuote2.Character);
        }

        [Fact]
        public async Task GetRandomQuote_CallsServiceExactlyOnce()
        {
            //Arrange
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetRandomQuote())
                .ReturnsNextFromSequence(
                    Task.FromResult<QuoteDto?>(
                        new QuoteDto { Value = "Test", Character = "Test Character" }
                    )
                );
            var controller = new QuotesController(fakeService);

            //Act
            await controller.GetRandomQuote();

            //Assert
            A.CallTo(() => fakeService.GetRandomQuote()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task GetRandomQuote_ReturnsCorrectStatusCode_WhenQuoteExists()
        {
            //Arrange
            var expectedQuote = new QuoteDto { Value = "Test Quote", Character = "Test Character" };
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetRandomQuote())
                .Returns(Task.FromResult<QuoteDto?>(expectedQuote));

            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetRandomQuote();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetRandomQuote_ReturnsCorrectMessage_WhenQuoteIsNull()
        {
            //Arrange
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetRandomQuote()).Returns(Task.FromResult<QuoteDto?>(null));

            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetRandomQuote();

            //Assert

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);

            var value = notFoundResult.Value;

            var dict = value!
                .GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(value, null));

            dict["type"].Should().Be("NotFound");
            dict["status"].Should().Be(404);
            dict["message"].Should().Be("No quotes available in the system.");
        }

        [Fact]
        public async Task GetByCharacterId_ReturnsExpectedQuotes()
        {
            //Arange
            List<QuoteDto> expected = new List<QuoteDto>
            {
                new QuoteDto
                {
                    Value = "Quote 1",
                    Character = "Character 1",
                    CharacterId = 1,
                },
                new QuoteDto
                {
                    Value = "Quote 2",
                    Character = "Character 1",
                    CharacterId = 1,
                },
            };

            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetByCharacterId(1))
                .Returns(Task.FromResult<List<QuoteDto>>(expected));

            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetByCharacterId(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<QuoteDto>>(okResult.Value);
            Assert.All(returned, quote => Assert.Equal(1, quote.CharacterId));
            returned.Should().OnlyContain(q => q.Character == "Character 1" && q.CharacterId == 1);
            returned.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByCharacterId_ReturnsNotFound_WhenNoQuotes()
        {
            //Arange
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetByCharacterId(1))
                .Returns(Task.FromResult<List<QuoteDto>>(new List<QuoteDto>()));

            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetByCharacterId(1);

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByCharacterId_ReturnsBadRequest_WhenCharacterIdIsZero()
        {
            //Arange
            var fakeService = A.Fake<IQuoteService>();
            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetByCharacterId(0);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByCharacterId_ReturnsBadRequest_WhenCharacterIdIsNegative()
        {
            //Arange
            var fakeService = A.Fake<IQuoteService>();
            var controller = new QuotesController(fakeService);

            //Act
            var result = await controller.GetByCharacterId(-5);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByCharacterId_DoesNotCallService_WhenCharacterIdIsZero()
        {
            // Arrange
            var fakeService = A.Fake<IQuoteService>();
            var controller = new QuotesController(fakeService);

            // Act
            var result = await controller.GetByCharacterId(0);

            // Assert
            A.CallTo(() => fakeService.GetByCharacterId(A<int>._)).MustNotHaveHappened();

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByCharacterId_ReturnsExpectedNumberOfQuotes()
        {
            // Arrange
            var expectedQuotes = new List<QuoteDto>
            {
                new QuoteDto
                {
                    Value = "Quote 1",
                    Character = "Character 1",
                    CharacterId = 1,
                },
                new QuoteDto
                {
                    Value = "Quote 2",
                    Character = "Character 1",
                    CharacterId = 1,
                },
                new QuoteDto
                {
                    Value = "Quote 3",
                    Character = "Character 1",
                    CharacterId = 1,
                },
            };

            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetByCharacterId(1))
                .Returns(Task.FromResult(expectedQuotes));

            var controller = new QuotesController(fakeService);

            // Act
            var result = await controller.GetByCharacterId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuotes = Assert.IsType<List<QuoteDto>>(okResult.Value);
            returnedQuotes.Should().HaveCount(expectedQuotes.Count);
        }

        [Fact]
        public async Task GetByQuery_ReturnsExpectedQuotes()
        {
            // Arrange
            string query = "test";
            List<QuoteDto> expected = new List<QuoteDto>
            {
                new QuoteDto { Value = "Test quote 1", Character = "Character 1" },
                new QuoteDto { Value = "Test quote 2", Character = "Character 2" },
            };

            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetByQuery(query)).Returns(Task.FromResult(expected));

            var controller = new QuotesController(fakeService);

            // Act
            var result = await controller.GetByQuery(query);

            // Then
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuotes = Assert.IsType<List<QuoteDto>>(okResult.Value);
            foreach (QuoteDto qdto in expected)
            {
                qdto.Value.ToLower().Should().Contain(query.ToLower());
            }
        }

        [Fact]
        public async Task GetByQuery_ReturnsNotFound_WhenNoQuotesFound()
        {
            // Arrange
            string query = "nonexistent";
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetByQuery(query))
                .Returns(Task.FromResult(new List<QuoteDto>()));

            var controller = new QuotesController(fakeService);

            // Act
            var result = await controller.GetByQuery(query);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetByQuery_ReturnsBadRequest_WhenQueryIsEmpty()
        {
            // Arrange
            string query = string.Empty;
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetByQuery(query))
                .Returns(Task.FromResult(new List<QuoteDto>()));
            var controller = new QuotesController(fakeService);

            // Act
            var result = await controller.GetByQuery(query);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsExpectedQuote_WhenIdIsRight()
        {
            // Arrange
            int id = 1;
            QuoteDto expected = new QuoteDto
            {
                Id = 1,
                Character = "Character 1",
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                UpdatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                CharacterId = 1,
                Value = "Quote 1",
            };
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetById(id)).Returns(Task.FromResult<QuoteDto?>(expected));
            var controller = new QuotesController(fakeService);
            // Act
            var result = await controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenIdQuoteDoesNotExist()
        {
            // Arrange
            int id = 2;
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetById(id)).Returns(Task.FromResult<QuoteDto?>(null));
            var controller = new QuotesController(fakeService);
            // Act
            var result = await controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenIdIsZeroOrNegative()
        {
            var fakeService = A.Fake<IQuoteService>();
            var controller = new QuotesController(fakeService);
            // Act
            var zeroResult = await controller.GetById(0);
            var negativeResult = await controller.GetById(-1);
            // Assert
            Assert.IsType<BadRequestObjectResult>(zeroResult.Result);
            Assert.IsType<BadRequestObjectResult>(negativeResult.Result);
        }

        [Fact]
        public async Task GetAll_ReturnsExpectedQuotes()
        {
            // Arrange
            List<QuoteDto> expected = new List<QuoteDto>
            {
                new QuoteDto { Value = "Quote 1", Character = "Character 1" },
                new QuoteDto { Value = "Quote 2", Character = "Character 2" },
            };
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetAll()).Returns(Task.FromResult(expected));
            var controller = new QuotesController(fakeService);
            // Act
            var result = await controller.GetAll();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuotes = Assert.IsType<List<QuoteDto>>(okResult.Value);
            returnedQuotes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoQuotes()
        {
            // Arrange
            var fakeService = A.Fake<IQuoteService>();
            A.CallTo(() => fakeService.GetAll()).Returns(Task.FromResult(new List<QuoteDto>()));
            var controller = new QuotesController(fakeService);
            // Act
            var result = await controller.GetAll();
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuotes = Assert.IsType<List<QuoteDto>>(okResult.Value);
            returnedQuotes.Should().BeEmpty();
        }
    }
}
