using System.Security.Claims;

namespace MyAPI.MyAuth
{
    // The logic for generating the access token, refresh token, and getting user details from the expired token
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
