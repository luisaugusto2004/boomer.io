using boomerio.Controllers;
using boomerio.DTOs.CharacterDTOs;
using boomerio.Services.CharacterService;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Tests
{
    public class CharacterControllerTests
    {
        [Fact]
        public async Task GetCharacterById_ShouldReturnCharacter_WhenCharacterExists()
        {
            // Arrange
            var fakeService = A.Fake<ICharacterService>();

            var expectedCharacter = new CharacterDto
            {
                Id = 1,
                Name = "Test Character",
                Franchise = "Test Franchise",
            };
            A.CallTo(() => fakeService.GetById(1))
                .Returns(Task.FromResult<CharacterDto?>(expectedCharacter));

            var controller = new CharactersController(fakeService);

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
            var fakeService = A.Fake<ICharacterService>();
            A.CallTo(() => fakeService.GetById(1)).Returns(Task.FromResult<CharacterDto?>(null));

            var controller = new CharactersController(fakeService);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetCharacterById_ShouldReturnBadRequest_WhenIdIsZeroOrNegative()
        {
            // Arrange
            var fakeService = A.Fake<ICharacterService>();
            var controller = new CharactersController(fakeService);

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
            var fakeService = A.Fake<ICharacterService>();

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
            A.CallTo(() => fakeService.GetAllAsync()).Returns(Task.FromResult(expectedCharacters));

            var controller = new CharactersController(fakeService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacters = Assert.IsType<List<CharacterDto>>(okResult.Value);
            actualCharacters.Should().BeEquivalentTo(expectedCharacters);
        }

        [Fact]
        public async Task GetAllCharacters_ShouldReturnEmptyList_WhenNoCharactersExist()
        {
            // Arrange
            var fakeService = A.Fake<ICharacterService>();
            A.CallTo(() => fakeService.GetAllAsync())
                .Returns(Task.FromResult(new List<CharacterDto>()));

            var controller = new CharactersController(fakeService);

            // Act
            var result = await controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacters = Assert.IsType<List<CharacterDto>>(okResult.Value);
            actualCharacters.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByFranchiseId_ShouldReturnFilteredCharacters_WhenCharactersExist()
        {
            // Arrange
            var fakeService = A.Fake<ICharacterService>();

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

            A.CallTo(() => fakeService.GetByFranchiseId(1))
                .Returns(Task.FromResult(expectedCharacters));

            var controller = new CharactersController(fakeService);

            // Act
            var result = await controller.GetByFranchiseId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualCharacters = Assert.IsType<List<CharacterDto>>(okResult.Value);

            actualCharacters.Should().OnlyContain(c => c.FranchiseId == 1);
            actualCharacters.Should().BeEquivalentTo(expectedCharacters);
        }

        [Fact]
        public async Task GetByFranchiseId_ShouldReturnBadRequest_WhenIdIsZeroOrNegative()
        {
            // Arrange
            var fakeService = A.Fake<ICharacterService>();
            var controller = new CharactersController(fakeService);

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
            var fakeService = A.Fake<ICharacterService>();
            A.CallTo(() => fakeService.GetByFranchiseId(1))
                .Returns(Task.FromResult(new List<CharacterDto>()));

            var controller = new CharactersController(fakeService);

            // Act
            var result = await controller.GetByFranchiseId(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
