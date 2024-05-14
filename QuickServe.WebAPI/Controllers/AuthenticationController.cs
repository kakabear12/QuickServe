using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickServe.Application.Interfaces;
using QuickServe.Application.Interfaces.Authenticates;
using QuickServe.Application.ViewModels.RefreshTokenDTO;
using QuickServe.Application.ViewModels.UserDTO;
using QuickServe.Domain.Entities;
using QuickServe.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace QuickServe.WebAPI.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IClaimsService claimsService;
        private int UserID => int.Parse(FindClaim(ClaimTypes.NameIdentifier));
        private string FindClaim(string claimName)
        {

            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(claimName);

            if (claim == null)
            {
                return null;
            }

            return claim.Value;

        }

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "For register")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserDTO registerObject)
        {
            var result = await _authenticationService.RegisterAsync(registerObject);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(result);
            }
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "For login")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AuthenUserDTO loginObject)
        {
            var result = await _authenticationService.LoginAsync(loginObject);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            else
            {
                return Ok(
                    new
                    {
                        success = result.Success,
                        message = result.Message,
                        token = result.Data
                    }
                );
            }
        }
        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "For get new access token")]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
           
            var response = await _authenticationService.RefreshToken(UserID, refreshTokenRequest);

            if (!response.Success)
            {
                return BadRequest(response);
            }
         
            return Ok(
                 new
                 {
                     success = response.Success,
                     message = response.Message,
                     token = response.Data
                 });
        }
        /*[Authorize]
        [HttpPost]
        [SwaggerOperation(Summary = "For logout")]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var logout = await userRepository.Logout(UserID);

            if (!logout.Success)
            {
                return UnprocessableEntity(logout);
            }
            // Xóa các claim đã được đặt trong HttpContext
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                identity.Claims.ToList().ForEach(c => identity.RemoveClaim(c));
            }
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var time = tokenGenerator.GetAccessTokenExpiration(accessToken);
            if (time >= DateTime.Now)
            {
                AccessTokenBlacklist tokenBlacklist = new AccessTokenBlacklist
                {
                    ExpiryDate = time,
                    Token = accessToken
                };
                userRepository.AddAccessTokenToBlacklist(tokenBlacklist);
            }
            return Ok("Logout successfully.");
        }*/
        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "For get information of current user.")]
       // [ServiceFilter(typeof(AccessTokenBlacklistFilter))]
        [Route("info")]
        public async Task<IActionResult> Info()
        {
            var response = await _authenticationService.GetProfilerAsync(UserID);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
    }
}
