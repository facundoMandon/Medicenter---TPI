using Application.Models;
using Application.Models.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // GET /Users - Solo administradores
        [HttpGet]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<List<UsersDTO>>> GetAll()
        {
            var users = await _usersService.GetAllAsync();
            return Ok(users.ToList());
        }

        // GET /Users/{id} - Usuario puede ver su propio perfil, o admin puede ver cualquiera
        [HttpGet("{id}")]
        [Authorize] // ⬅️ Requiere autenticación
        public async Task<ActionResult<UsersDTO>> GetById([FromRoute] int id)
        {
            // Verificar que el usuario autenticado sea el mismo o sea admin
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != id)
            {
                return Forbid(); // 403 Forbidden
            }

            var user = await _usersService.GetByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        // PUT /Users/profile/{userId} (editarPerfil) - Usuario puede editar su propio perfil
        [HttpPut("profile/{userId}")]
        [Authorize] // ⬅️ Requiere autenticación
        public async Task<ActionResult<UsersDTO>> EditProfile([FromRoute] int userId, [FromBody] CreationUsersDTO dto)
        {
            // Verificar que el usuario autenticado sea el mismo
            var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (authenticatedUserId != userId)
            {
                return Forbid(); // 403 Forbidden
            }

            var updated = await _usersService.UpdateAsync(userId, dto);
            if (updated == null)
                return NotFound("User not found.");

            return Ok(updated);
        }

        // DELETE /Users/account/{userId} (eliminarCuenta) - Usuario puede eliminar su propia cuenta
        [HttpDelete("account/{userId}")]
        [Authorize] // ⬅️ Requiere autenticación
        public async Task<ActionResult> DeleteAccount([FromRoute] int userId)
        {
            // Verificar que el usuario autenticado sea el mismo
            var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (authenticatedUserId != userId)
            {
                return Forbid(); // 403 Forbidden
            }

            await _usersService.DeleteAccountAsync(userId);
            return NoContent();
        }

        // POST /Users/password/recover (recuperarContraseña) - Público
        [HttpPost("password/recover")]
        [AllowAnonymous] // ⬅️ Público (recuperar contraseña)
        public async Task<ActionResult> RecoverPassword([FromBody] string email)
        {
            await _usersService.RecoverPasswordAsync(email);
            return Accepted();
        }
    }
}