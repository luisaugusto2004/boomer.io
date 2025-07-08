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

        [HttpGet]
        public async Task<ActionResult<List<FranchiseDto>>> GetAll()
        {
            var franchises = await _franchiseService.GetAllAsync();
            return Ok(franchises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FranchiseDto>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(
                    new
                    {
                        type = "BadRequest",
                        status = 400,
                        message = "Invalid franchise ID.",
                    }
                );
            }
            var franchise = await _franchiseService.GetById(id);
            if (franchise == null)
            {
                return NotFound(
                    new
                    {
                        type = "NotFound",
                        status = 404,
                        message = "Franchise not found.",
                    }
                );
            }
            return Ok(franchise);
        }
    }
}
