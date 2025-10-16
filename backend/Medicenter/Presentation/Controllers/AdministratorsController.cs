using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdministratorsController : ControllerBase
    {
        private readonly IAdministratorsService _adminsService;

        public AdministratorsController(IAdministratorsService adminsService)
        {
            _adminsService = adminsService;
        }

        // POST /admin/administrators (Creación de Administrador)
        [HttpPost("administrators")]
        public async Task<ActionResult<AdministratorsDTO>> CreateAdministrator([FromBody] CreationUsersDTO dto)
        {
            if (dto.rol != Domain.Enums.Roles.Administrator)
            {
                return BadRequest("Invalid role for this endpoint.");
            }
            var created = await _adminsService.CreateAdministratorAsync(dto);
            // Redirige al método GetById del controlador Users
            return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = created.Id }, created);
        }

        // PUT /admin/users/{userId} (ModificarUsuario)
        [HttpPut("users/{userId}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int userId, [FromBody] CreationUsersDTO dto)
        {
            try
            {
                await _adminsService.UpdateUserAsync(userId, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with ID {userId} not found.");
            }
        }

        // DELETE /admin/users/{userId} (EliminarUsuario)
        [HttpDelete("users/{userId}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            await _adminsService.DeleteUserAsync(userId);
            return NoContent();
        }

        // GET /admin/professionals (verProfesionales)
        [HttpGet("professionals")]
        public async Task<ActionResult<IEnumerable<ProfessionalsDTO>>> ViewProfessionals()
        {
            var professionals = await _adminsService.ViewProfessionalsAsync();
            return Ok(professionals);
        }
    }
}