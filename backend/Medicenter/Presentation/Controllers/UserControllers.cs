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
    public class UserController : ControllerBase
    {
        private readonly IUserService _usersService;

        public UserController(IUserService usersService)
        {
            _usersService = usersService;
        }

        // GET /User - Solo administradores
        [HttpGet]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult<List<UserDTO>>> GetAll()
        {
            var users = await _usersService.GetAllAsync();
            return Ok(users.ToList());
        }

        // GET /User/{id} - Usuario puede ver su propio perfil, o admin puede ver cualquiera
        [HttpGet("{id}")]
        [Authorize] // ⬅ Requiere autenticación
        public async Task<ActionResult<UserDTO>> GetById([FromRoute] int id)
        {
            // Verificar que el usuario autenticado sea el mismo o sea admin
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != id)
            {
                return Forbid(); // 403 Forbidden
            }

            // El middleware manejará la excepción si el usuario no existe
            var user = await _usersService.GetByIdAsync(id);
            return Ok(user);
        }

        // PUT /User/profile/{userId} (editarPerfil) - Usuario puede editar su propio perfil
        [HttpPut("profile/{userId}")]
        [Authorize] // ⬅ Requiere autenticación
        public async Task<ActionResult<UserDTO>> EditProfile([FromRoute] int userId, [FromBody] CreationUserDTO dto)
        {
            // Verificar que el usuario autenticado sea el mismo
            var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (authenticatedUserId != userId)
            {
                return Forbid(); // 403 Forbidden
            }

            // El middleware manejará las excepciones de validación o si el usuario no existe
            var updated = await _usersService.UpdateAsync(userId, dto);
            return Ok(updated);
        }

        // DELETE /User/account/{userId} (eliminarCuenta) - Usuario puede eliminar su propia cuenta
        [HttpDelete("account/{userId}")]
        [Authorize] // ⬅ Requiere autenticación
        public async Task<ActionResult> DeleteAccount([FromRoute] int userId)
        {
            // Verificar que el usuario autenticado sea el mismo
            var authenticatedUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (authenticatedUserId != userId)
            {
                return Forbid(); // 403 Forbidden
            }

            // El middleware manejará la excepción si el usuario no existe
            await _usersService.DeleteAccountAsync(userId);
            return NoContent();
        }

        // POST /User/password/recover (recuperarContraseña) - Público
        [HttpPost("password/recover")]
        [AllowAnonymous] // ⬅ Público (recuperar contraseña)
        public async Task<ActionResult> RecoverPassword([FromBody] string email)
        {
            // El middleware manejará las excepciones de validación
            await _usersService.RecoverPasswordAsync(email);
            return Accepted();
        }
    }
}