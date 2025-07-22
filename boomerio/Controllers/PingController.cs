using Microsoft.AspNetCore.Mvc;

namespace boomerio.Controllers
{
    [Route("ping")]
    [ApiController]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Returns a simple "pong" response to indicate that the API is running.
        /// </summary>
        /// <response code="200">Returns "pong".</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet, HttpHead]
        public IActionResult Ping()=> Ok("pong");
    }
}
