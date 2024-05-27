using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pc_club_server.API.Models;
using pc_club_server.Core;
using pc_club_server.Services.PlayerSessionService;
using Swashbuckle.AspNetCore.Annotations;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/play-session")]
    [Authorize]
    [SwaggerTag("Play session")]
    public class PlaySessionController : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(
            Summary = "Start play session",
            Description = "Start play session")]
        public async Task<ActionResult<PlaySessionDto>> StartPlaySession(
            [FromQuery]
            [SwaggerParameter(
                Description = "Time when session ends. Send local time, without timezone. Example: 2024-05-24T21:00:00",
                Required = true
            )]
            DateTime endTime,
            [FromServices]
            IPlaySessionService playSessionService)
        {
            var session = await playSessionService.StartPlaySession(User.GetID(), endTime);
            if (session == null)
                return BadRequest();
            return Ok(session);
        }

        [Obsolete("Not implemented")]
        [HttpPost("end")]
        [SwaggerOperation(
            Summary = "End play session",
            Description = "End play session")]
        public async Task<ActionResult<PlaySessionDto>> EndPlaySession(
            [FromServices] IPlaySessionService playSessionService)
        {
            return Ok(await playSessionService.EndPlaySession(User.GetID()));
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get current play session",
            Description = "Get current play session")]
        public async Task<ActionResult<PlaySessionDto>> GetPlaySession(
            [FromServices] IPlaySessionService playSessionService)
        {
            var session = await playSessionService.GetPlaySession(User.GetID());
            if (session == null)
                return NotFound();
            return Ok(session);
        }
    }
}
