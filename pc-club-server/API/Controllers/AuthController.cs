using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pc_club_server.API.Models;
using pc_club_server.Infrastructure.Database;
using pc_club_server.Services.JwtService;
using pc_club_server.Services.UserService;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController(
        PcClubDbContext context,
        ILogger<AuthController> logger) : ControllerBase
    {
        private readonly PcClubDbContext _context = context;
        private readonly ILogger<AuthController> _logger = logger;
        
        [HttpPost]
        [Route("login")]
        public IActionResult GetToken(
            [FromQuery] UserInfo requestUser,
            [FromServices] IUserService userService,
            [FromServices] IJwtService jwtService)
        {
            var user = userService.GetUser(requestUser.Username);
            if (user == null)
                return Unauthorized();

            if (!userService.IsAuthenticated(requestUser.Password, user.Password))
                return Unauthorized();

            var token = jwtService.GenerateToken(user);
            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult RegisterUser(
            [FromQuery] UserInfo requestUser,
            [FromServices] IUserService userService,
            [FromServices] IJwtService jwtService)
        {
            var user = userService.RegisterUser(requestUser);
            if (user == null)
                return BadRequest();

            var token = jwtService.GenerateToken(user);
            return Ok(token);
        }
    }
}