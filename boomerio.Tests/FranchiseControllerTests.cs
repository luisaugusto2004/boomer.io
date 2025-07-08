using boomerio.Controllers;
using boomerio.DTOs.FranchiseDTOs;
using boomerio.Services.FranchiseService;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Tests
{
    public class FranchiseControllerTests
    {
        [Fact]
        public async Task GetAllFranchises_ReturnsListOfFranchises()
        {
            var fakeService = A.Fake<IFranchiseService>();
            var expectedFranchises = new List<FranchiseDto>
            {
                new FranchiseDto { Id = 1, Name = "Test Franchise 1" },
                new FranchiseDto { Id = 2, Name = "Test Franchise 2" },
            };
            A.CallTo(() => fakeService.GetAllAsync()).Returns(Task.FromResult(expectedFranchises));
            var controller = new FranchisesController(fakeService);
            var result = await controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualFranchises = Assert.IsType<List<FranchiseDto>>(okResult.Value);
            actualFranchises.Should().BeEquivalentTo(expectedFranchises);
        }

        [Fact]
        public async Task GetAllFranchises_ReturnsEmptyList_WhenNoFranchisesExist()
        {
            var fakeService = A.Fake<IFranchiseService>();
            A.CallTo(() => fakeService.GetAllAsync())
                .Returns(Task.FromResult(new List<FranchiseDto>()));
            var controller = new FranchisesController(fakeService);
            var result = await controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualFranchises = Assert.IsType<List<FranchiseDto>>(okResult.Value);
            actualFranchises.Should().BeEmpty();
        }

        [Fact]
        public async Task GetFranchiseById_ReturnsFranchise_WhenExists()
        {
            var fakeService = A.Fake<IFranchiseService>();
            var expectedFranchise = new FranchiseDto { Id = 1, Name = "Test Franchise" };
            A.CallTo(() => fakeService.GetById(1))
                .Returns(Task.FromResult<FranchiseDto?>(expectedFranchise));
            var controller = new FranchisesController(fakeService);
            var result = await controller.GetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualFranchise = Assert.IsType<FranchiseDto>(okResult.Value);
            actualFranchise.Should().BeEquivalentTo(expectedFranchise);
        }

        [Fact]
        public async Task GetFranchiseById_ReturnsNotFound_WhenIdDoesNotExist()
        {
            var fakeService = A.Fake<IFranchiseService>();
            A.CallTo(() => fakeService.GetById(1)).Returns(Task.FromResult<FranchiseDto?>(null));
            var controller = new FranchisesController(fakeService);
            var result = await controller.GetById(1);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetFranchiseById_ReturnsBadRequest_WhenIdIsZeroOrNegative()
        {
            var fakeService = A.Fake<IFranchiseService>();
            var controller = new FranchisesController(fakeService);
            var zeroResult = await controller.GetById(0);
            var negativeResult = await controller.GetById(-1);
            var zeroBadRequest = Assert.IsType<BadRequestObjectResult>(zeroResult.Result);
            var negativeBadRequest = Assert.IsType<BadRequestObjectResult>(negativeResult.Result);
            zeroBadRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            negativeBadRequest.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
