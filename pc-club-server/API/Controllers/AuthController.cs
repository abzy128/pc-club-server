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
    [Authorize]
    public class AuthController(
        PcClubDbContext context,
        ILogger<AuthController> logger) : ControllerBase
    {
        private readonly PcClubDbContext _context = context;
        private readonly ILogger<AuthController> _logger = logger;
        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetToken(
            [FromBody] UserInfo requestUser,
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
    }
}