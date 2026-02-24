using Microsoft.AspNetCore.Mvc;
using WongaApi.DTOs;
using WongaApi.Services;

namespace WongaApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var token = await _authService.RegisterAsync(request);
            if (token == null)
                return Conflict(new { message = "Email already in use." });

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            if (token == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(new { token });
        }
    }
}