using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pc_club_server.API.Models;
using pc_club_server.Services.SteamService;
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
        [Route("game-list/{steamID}")]
        [SwaggerOperation(
            Summary = "Get game list",
            Description = "Get game list by steam id")]
        public async Task<ActionResult<UserGameDataModel>> GetGameList(
            ISteamService steamService,
            [SwaggerParameter(Required = true)] string steamID)
        {
            if (steamID.IsNullOrEmpty())
                return BadRequest();

            return Ok(await steamService.GetUserGames(steamID));
        }
    }
}
