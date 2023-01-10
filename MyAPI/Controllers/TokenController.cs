using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Data;
using MyAPI.Models;
using MyAPI.MyAuth;

namespace MyAPI.Controllers
{
    /// <summary>
    /// A refresh endpoint, which gets the user information from the expired access token and validates the refresh token against the use
    /// </summary>
    [ApiController]
    [Route("api/[controller")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserContext _userContext;

        public TokenController(ITokenService tokenService, UserContext userContext)
        {
            this._tokenService = tokenService;
            this._userContext = userContext;
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid cliet request");

            string? accessToken = tokenApiModel.AccessToken;
            string? refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name; // this is mapped to the Name claim by default

            var user = _userContext.LoginModels.Where(u => u.UserName.Equals(userName)).FirstOrDefault();

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            _userContext.SaveChanges();

            return Ok(new AuthenticatedResponse()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;

            var user = _userContext.LoginModels.SingleOrDefault(u => u.UserName == username);

            if (user == null) return BadRequest();

            user.RefreshToken = null;

            _userContext.SaveChanges();

            return NoContent();
        }
    }
}
