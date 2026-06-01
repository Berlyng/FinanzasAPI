using FinananzasAPI.Application.DTOs.Auth;
using FinananzasAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinananzasAPI.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(new { message = ex.Message });
            }
      
        }
    }
}
