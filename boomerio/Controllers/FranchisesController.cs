using boomerio.DTOs;
using boomerio.DTOs.FranchiseDTOs;
using boomerio.Services.FranchiseService;
using Microsoft.AspNetCore.Mvc;

namespace boomerio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FranchisesController : ControllerBase
    {
        private readonly IFranchiseService _franchiseService;

        public FranchisesController(IFranchiseService franchiseService)
        {
            _franchiseService = franchiseService;
        }

        /// <summary>
        /// Retrieves all franchises.
        /// If no franchises are available, it returns an empty list.
        /// </summary>
        /// <response code="200">Returns a list of franchises.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(List<FranchiseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<List<FranchiseDto>>> GetAll()
        {
            var franchises = await _franchiseService.GetAllAsync();
            return Ok(franchises);
        }

        /// <summary>
        /// Retrieves a franchise by its ID.
        /// </summary>
        /// <param name="id">The ID of the franchise to retrieve.</param>
        /// <response code="200">Returns the franchise with the specified ID.</response>
        /// <response code="400">If the ID is less than or equal to zero.</response>
        /// <response code="404">If the franchise with the specified ID does not exist.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [ProducesResponseType(typeof(FranchiseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseDto>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ApiError("BadRequest", 400, "ID must be greater than zero."));
            }
            var franchise = await _franchiseService.GetById(id);
            if (franchise == null)
            {
                return NotFound(
                    new ApiError("NotFound", 404, $"Franchise not found for the id {id}.")
                );
            }
            return Ok(franchise);
        }
    }
}
