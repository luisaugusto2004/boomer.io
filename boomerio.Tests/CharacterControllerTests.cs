using boomerio.Controllers;
using boomerio.DTOs;
using boomerio.DTOs.CharacterDTOs;
using boomerio.DTOs.FranchiseDTOs;
using boomerio.Models;
using boomerio.Services.CharacterService;
using boomerio.Services.FranchiseService;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Tests
{
    public class CharacterControllerTests
    {
        [Fact]
        public async Task GetCharacterById_ShouldReturnCharacter_WhenCharacterExists()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();

            var expectedCharacter = new CharacterDto
            {
                Id = 1,
                Name = "Test Character",
                Franchise = "Test Franchise",
            };
            A.CallTo(() => fakeCharacterService.GetById(1))
                .Returns(Task.FromResult<CharacterDto?>(expectedCharacter));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacter = Assert.IsType<CharacterDto?>(okResult.Value);
            actualCharacter.Should().BeEquivalentTo(expectedCharacter);
        }

        [Fact]
        public async Task GetCharacterById_ShouldReturnNotFound_WhenCharacterDoesNotExist()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();
            A.CallTo(() => fakeCharacterService.GetById(1)).Returns(Task.FromResult<CharacterDto?>(null));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

            var error = Assert.IsType<ApiError>(notFoundResult.Value);
            A.CallTo(() => fakeCharacterService.GetByFranchiseId(A<int>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task GetCharacterById_ShouldReturnBadRequest_WhenIdIsZeroOrNegative()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();
            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var zeroResult = await controller.GetById(0);
            var negativeResult = await controller.GetById(-1);

            // Assert
            var zeroBadRequest = Assert.IsType<BadRequestObjectResult>(zeroResult.Result);
            var negativeBadRequest = Assert.IsType<BadRequestObjectResult>(negativeResult.Result);
            zeroBadRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            negativeBadRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GetAllCharacters_ShouldReturnAllCharacters_WhenCharactersExist()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();

            var expectedCharacters = new List<CharacterDto>
            {
                new CharacterDto
                {
                    Id = 1,
                    Name = "Character 1",
                    Franchise = "Franchise 1",
                },
                new CharacterDto
                {
                    Id = 2,
                    Name = "Character 2",
                    Franchise = "Franchise 2",
                },
            };
            A.CallTo(() => fakeCharacterService.GetAllAsync()).Returns(Task.FromResult<IEnumerable<CharacterDto>>(expectedCharacters));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacters = Assert.IsAssignableFrom<IEnumerable<CharacterDto>>(okResult.Value);
            actualCharacters.Should().BeEquivalentTo(expectedCharacters);
        }

        [Fact]
        public async Task GetAllCharacters_ShouldReturnEmptyList_WhenNoCharactersExist()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();
            A.CallTo(() => fakeCharacterService.GetAllAsync())
                .Returns(Task.FromResult<IEnumerable<CharacterDto>>(new List<CharacterDto>()));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacters = Assert.IsAssignableFrom<IEnumerable<CharacterDto>>(okResult.Value);
            actualCharacters.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByFranchiseId_ShouldReturnFilteredCharacters_WhenCharactersExist()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();

            var expectedCharacters = new List<CharacterDto>
            {
                new CharacterDto
                {
                    Id = 1,
                    Name = "Character 1",
                    Franchise = "Franchise 1",
                    FranchiseId = 1,
                },
                new CharacterDto
                {
                    Id = 2,
                    Name = "Character 2",
                    Franchise = "Franchise 1",
                    FranchiseId = 1,
                },
            };

            A.CallTo(() => fakeCharacterService.GetByFranchiseId(1))
                .Returns(Task.FromResult<IEnumerable<CharacterDto>>(expectedCharacters));
            A.CallTo(() => fakeFranchiseService.Exists(1))
                .Returns(Task.FromResult<bool>(true));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var result = await controller.GetByFranchiseId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacters = Assert.IsAssignableFrom<IEnumerable<CharacterDto>>(okResult.Value);

            actualCharacters.Should().OnlyContain(c => c.FranchiseId == 1);
            actualCharacters.Should().BeEquivalentTo(expectedCharacters);
        }

        [Fact]
        public async Task GetByFranchiseId_ShouldReturnBadRequest_WhenIdIsZeroOrNegative()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();
            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var zeroResult = await controller.GetByFranchiseId(0);
            var negativeResult = await controller.GetByFranchiseId(-1);

            // Assert
            var zeroBadRequest = Assert.IsType<BadRequestObjectResult>(zeroResult.Result);
            var negativeBadRequest = Assert.IsType<BadRequestObjectResult>(negativeResult.Result);
            zeroBadRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            negativeBadRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task GetByFranchiseId_ShouldReturnNotFound_WhenFranchiseDoesNotExist()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();

            A.CallTo(() => fakeFranchiseService.Exists(1))
                .Returns(Task.FromResult<bool>(false));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);

            // Act
            var result = await controller.GetByFranchiseId(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);

            var error = Assert.IsType<ApiError>(notFoundResult.Value);
            error.Status.Should().Be(404);

            A.CallTo(() => fakeCharacterService.GetByFranchiseId(A<int>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task GetByFranchiseId_ShouldReturnEmptyList_WhenFranchiseExistsButNoCharacters()
        {
            // Arrange
            var fakeCharacterService = A.Fake<ICharacterService>();
            var fakeFranchiseService = A.Fake<IFranchiseService>();

            A.CallTo(() => fakeFranchiseService.Exists(1))
                .Returns(Task.FromResult<bool>(true));
            A.CallTo(() => fakeCharacterService.GetByFranchiseId(1))
                .Returns(Task.FromResult<IEnumerable<CharacterDto>>(new List<CharacterDto>()));

            var controller = new CharactersController(fakeCharacterService, fakeFranchiseService);
            //Act
            var result = await controller.GetByFranchiseId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var characters = Assert.IsAssignableFrom<IEnumerable<CharacterDto>>(okResult.Value);

            characters.Should().BeEmpty();
        }
    }
}
