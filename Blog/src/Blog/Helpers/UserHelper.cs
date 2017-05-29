using System.Linq;
using System.Security.Claims;

namespace Blog.Helpers
{
    public static class UserHelper
    {
        public static string GetLoggedUserId(this ClaimsPrincipal user)
        {
            var res = string.Empty;
            var claims = user.Identity as ClaimsIdentity;
            if (claims != null)
            {
                var userIdClaim = claims.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    res = userIdClaim.Value;
                }
            }
            return res;
        }
    }
}
