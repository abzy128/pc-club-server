using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pc_club_server.Services.UserService;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class ProfileConroller : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProfile(
            [FromServices] IUserService userService)
        {
            return Ok();
        }
    }
}
