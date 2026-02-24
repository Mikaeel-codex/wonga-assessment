using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WongaApi.Services;

namespace WongaApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;

        public UserController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return Unauthorized();

            var user = await _authService.GetUserAsync(email);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}