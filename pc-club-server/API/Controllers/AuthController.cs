using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pc_club_server.API.Models;
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
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(401, "Unauthorized")]
        public async Task<IActionResult> Login(
            [FromQuery] UserInfo requestUser,
            [FromServices] IUserService userService,
            [FromServices] IJwtService jwtService)
        {
            if (requestUser.Username.IsNullOrEmpty() || requestUser.Password.IsNullOrEmpty())
                return BadRequest();
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
        [SwaggerResponse(200, "OK")]
        [SwaggerResponse(201, "Registraion successful")]
        [SwaggerResponse(400, "Bad request")]
        [SwaggerResponse(409, "User already exists")]
        public async Task<IActionResult> RegisterUser(
            [FromQuery] UserInfo requestUser,
            [FromServices] IUserService userService,
            [FromServices] IJwtService jwtService)
        {
            if (requestUser.Username.IsNullOrEmpty() || requestUser.Password.IsNullOrEmpty())
                return BadRequest("Check fields");
            requestUser = new UserInfo(requestUser.Username!.Trim(), requestUser.Password!.Trim());
            if(requestUser.Username!.Contains(' ') || requestUser.Password!.Contains(' '))
                return BadRequest("Username cannot contain spaces");
            var user = await userService.RegisterUser(requestUser);
            if (user == null)
                return Conflict("User already exists");

            var token = jwtService.GenerateToken(user);
            return CreatedAtAction(nameof(Login), token);
        }
    }
}