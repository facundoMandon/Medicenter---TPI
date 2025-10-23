using Application.Models;
using Application.Models.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET /Users
        [HttpGet]
        public async Task<ActionResult<List<UsersDTO>>> GetAll()
        {
            var users = await _usersService.GetAllAsync();
            return Ok(users.ToList());
        }

        // GET /Users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsersDTO>> GetById([FromRoute] int id)
        {
            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // PUT /Users/profile/{userId} (editarPerfil)
        [HttpPut("profile/{userId}")]
        public async Task<ActionResult<UsersDTO>> EditProfile([FromRoute] int userId, [FromBody] CreationUsersDTO dto)
        {
            var updated = await _usersService.UpdateAsync(userId, dto);
            if (updated == null)
                return NotFound("User not found.");

            return Ok(updated);
        }

        // DELETE /Users/account/{userId} (eliminarCuenta)
        [HttpDelete("account/{userId}")]
        public async Task<ActionResult> DeleteAccount([FromRoute] int userId)
        {
            await _usersService.DeleteAccountAsync(userId);
            return NoContent();
        }

        // POST /Users/password/recover (recuperarContraseña)
        [HttpPost("password/recover")]
        public async Task<ActionResult> RecoverPassword([FromBody] string email)
        {
            await _usersService.RecoverPasswordAsync(email);
            return Accepted();
        }
    }
}