using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pc_club_server.API.Models;
using pc_club_server.Infrastructure.Database;
using pc_club_server.Services.JwtService;
using pc_club_server.Services.UserService;
using Swashbuckle.AspNetCore.Annotations;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    [SwaggerTag("Authentication")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [SwaggerOperation(
            Summary = "Get JWT token",
            Description = "Get JWT")]
        public async Task<IActionResult> GetToken(
            [FromQuery] UserInfo requestUser,
            [FromServices] IUserService userService,
            [FromServices] IJwtService jwtService)
        {
            var user = await userService.GetUser(requestUser.Username);
            if (user == null)
                return Unauthorized();

            if (!userService.IsAuthenticated(requestUser.Password, user.Password))
                return Unauthorized();

            var token = jwtService.GenerateToken(user);
            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        [SwaggerOperation(
            Summary = "Register new user",
            Description = "Register new user")]
        public async Task<IActionResult> RegisterUser(
            [FromQuery] UserInfo requestUser,
            [FromServices] IUserService userService,
            [FromServices] IJwtService jwtService)
        {
            var user = await userService.RegisterUser(requestUser);
            if (user == null)
                return BadRequest();

            var token = jwtService.GenerateToken(user);
            return Ok(token);
        }
    }
}