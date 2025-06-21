using boomer_shooter_api.Services.FranchiseService;
using boomer_shooter_api.DTOs.FranchiseDTOs;
using Microsoft.AspNetCore.Mvc;

namespace boomer_shooter_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FranchiseController : ControllerBase
    {
        private readonly IFranchiseService _franchiseService;

        public FranchiseController(IFranchiseService franchiseService)
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
            var franchise = await _franchiseService.GetById(id);
            if (franchise == null)
            {
                return NotFound(new
                {
                    type = "NotFound",
                    status = 404,
                    message = "Franchise not found."
                });
            }
            return Ok(franchise);
        }
    }
}