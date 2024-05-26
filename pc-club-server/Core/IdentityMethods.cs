using System.Security.Claims;

namespace pc_club_server.Core
{
    public static class IdentityMethods
    {
        public static int GetID(this ClaimsPrincipal user)
        {
            var claimsIdentity = user.Identity as ClaimsIdentity;
            var claim = claimsIdentity!.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim!.Value);
        }
    }
}
