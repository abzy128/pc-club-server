using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pc_club_server.API.Models;
using pc_club_server.Core;
using pc_club_server.Services.UserService;
using Swashbuckle.AspNetCore.Annotations;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    [SwaggerTag("User profile")]
    public class ProfileConroller : ControllerBase
    {
        [HttpGet]
        [SwaggerOperation(
           Summary = "Get current user info",
           Description = "Get current user info")]
        public async Task<ActionResult<UserDto>> GetUser(
            [FromServices] IUserService userService)
        {
            return Ok(await userService.GetUser(User.GetID()));
        }
        
        [HttpPost]
        [SwaggerOperation(
           Summary = "Update user info",
           Description = "Update user info")]
        public async Task<ActionResult<UserDto>> UpdateUser(
            [FromQuery] UserUpdateDto user,
            [FromServices] IUserService userService)
        {
            return Ok(await userService.UpdateUser(User.GetID(), user));
        }
        
        [HttpPost("update-password")]
        [SwaggerOperation(
          Summary = "Update user password",
          Description = "Update user password")]
        public async Task<ActionResult> UpdatePassword(
            [FromQuery] string password,
            [FromServices] IUserService userService)
        {
            var isUpdateSuccessful = await userService.UpdatePassword(User.GetID(), password);
            if(!isUpdateSuccessful)
                return BadRequest();
            return Ok();
        }
    }
}
