using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomAuthenticationService _customAuthenticationService;

        public AuthenticationController(ICustomAuthenticationService customAuthenticationService)
        {
            _customAuthenticationService = customAuthenticationService;
        }

        /// Endpoint de login - Retorna el token JWT
        /// POST: api/Authentication
        [HttpPost]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticationRequestDTO authenticationRequestDTO)
        {
            try
            {
                string newToken = await _customAuthenticationService.AuthenticateAsync(authenticationRequestDTO);
                return Ok(new { token = newToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}