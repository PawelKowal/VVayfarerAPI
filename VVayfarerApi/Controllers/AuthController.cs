using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VVayfarerApi.Data;
using VVayfarerApi.Models;
using VVayfarerApi.Repositories;

namespace VVayfarerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUnitOfWork _uow;

        public AuthController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // /api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _uow.UserRepository.RegisterUserAsync(model);

                if (result.IsSuccess)
                {
                    SetTokenCookie(result.RefreshToken);

                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest(ModelState);
        }

        // /api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _uow.UserRepository.LoginUserAsync(model);

                if (result.IsSuccess)
                {
                    SetTokenCookie(result.RefreshToken);

                    return Ok(result);
                }

                return BadRequest("Some properties are not valid.");
            }

            return BadRequest("Some properties are not valid.");
        }

        // /api/auth/refresh-token
        [HttpGet("Refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            if (ModelState.IsValid)
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var result = await _uow.UserRepository.RefreshTokenAsync(refreshToken);

                if (result.IsSuccess)
                {
                    SetTokenCookie(result.RefreshToken);

                    return Ok(result);
                }

                return Unauthorized(new { message = "Invalid token" });
            }

            return NotFound();
        }

        // /api/auth/logout
        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            var authorizedUserId = User.FindFirst(ClaimTypes.NameIdentifier);

            if (authorizedUserId == null)
            {
                return Unauthorized();
            }

            var result = await _uow.UserRepository.LogoutUserAsync(authorizedUserId.Value);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        //helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                SameSite = SameSiteMode.Lax,
                Secure = true
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
