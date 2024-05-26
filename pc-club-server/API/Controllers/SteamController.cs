using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pc_club_server.API.Models;
using pc_club_server.Core;
using pc_club_server.Services.SteamService;
using pc_club_server.Services.UserService;
using Swashbuckle.AspNetCore.Annotations;

namespace pc_club_server.API.Controllers
{
    [ApiController]
    [Route("api/steam")]
    [Authorize]
    public class SteamController : ControllerBase
    {
        [HttpGet]
        [Route("banner/{gameId}")]
        [SwaggerOperation(
           Summary = "Get game asset",
           Description = "Get game asset by game id")]
        public ActionResult<string> GetBanner(
            ISteamService steamService,
            [SwaggerParameter(Required = true)] int gameId)
        {
            if (gameId < 0)
                return BadRequest();

            return Ok(steamService.GetGameBannerURI(gameId));
        }

        [HttpGet]
        [Route("game-list")]
        [SwaggerOperation(
            Summary = "Get game list",
            Description = "Get game list by steam id")]
        public async Task<ActionResult<UserGameDataModel>> GetGameList(
            ISteamService steamService,
            IUserService userService)
        {
            var user = await userService.GetUser(User.GetID());
            if(user == null || user.SteamID == null)
                return BadRequest("SteamID is not set");
            return Ok(await steamService.GetUserGames(user.SteamID));
        }
    }
}
