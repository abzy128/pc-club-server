using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pc_club_server.API.Models;
using pc_club_server.Services.UserService;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class ProfileConroller : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
           Summary = "Get current user info",
           Description = "Get current user info")]
        public async Task<IActionResult> GetUser(
            [FromServices] IUserService userService)
        {
            return Ok(await userService.GetUser(User.Identity!.Name));
        }
        
        [HttpPost]
        [SwaggerOperation(
           Summary = "Update user info",
           Description = "Update user info")]
        public async Task<IActionResult> UpdateUser(
            [FromQuery] UserUpdateDto user,
            [FromServices] IUserService userService)
        {
            return Ok(await userService.UpdateUser(GetUserIdentifier(), user));
        }
        
        [HttpPost("update-password")]
        [SwaggerOperation(
          Summary = "Update user password",
          Description = "Update user password")]
        public async Task<IActionResult> UpdatePassword(
            [FromQuery] string password,
            [FromServices] IUserService userService)
        {
            return Ok(await userService.UpdatePassword(GetUserIdentifier(), password));
        }
        private int GetUserIdentifier()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim!.Value);
        }
    }
}
