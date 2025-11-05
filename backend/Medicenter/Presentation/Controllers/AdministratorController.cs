using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Roles = "Administrator")] // ⬅ Solo administradores pueden acceder
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _adminsService;

        public AdministratorController(IAdministratorService adminsService)
        {
            _adminsService = adminsService;
        }

        // POST /admin/administrators (Creación de Administrador)
        [HttpPost("administrators")]
        public async Task<ActionResult<AdministratorDTO>> CreateAdministrator([FromBody] CreationUserDTO dto)
        {
            // El middleware manejará las excepciones de validación (incluyendo validación de rol)
            var created = await _adminsService.CreateAdministratorAsync(dto);
            return CreatedAtAction(nameof(UserController.GetById), "User", new { id = created.Id }, created);
        }

        // PUT /admin/users/{userId} (ModificarUsuario)
        [HttpPut("users/{userId}")]
        public async Task<ActionResult> UpdateUser([FromRoute] int userId, [FromBody] CreationUserDTO dto)
        {
            // El middleware manejará las excepciones de validación o si el usuario no existe
            await _adminsService.UpdateUserAsync(userId, dto);
            return NoContent();
        }

        // DELETE /admin/users/{userId} (EliminarUsuario)
        [HttpDelete("users/{userId}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            // El middleware manejará la excepción si el usuario no existe
            await _adminsService.DeleteUserAsync(userId);
            return NoContent();
        }

        // GET /admin/professionals (verProfesionales)
        [HttpGet("professionals")]
        public async Task<ActionResult<IEnumerable<ProfessionalDTO>>> ViewProfessional()
        {
            var professionals = await _adminsService.ViewProfessionalAsync();
            return Ok(professionals);
        }

        // GET /admin/specialties (verEspecialidades)
        [HttpGet("specialties")]
        public async Task<ActionResult<IEnumerable<SpecialtiesDTO>>> ViewSpecialties()
        {
            var specialties = await _adminsService.ViewSpecialtiesAsync();
            return Ok(specialties);
        }

        // DELETE /admin/specialties/{specialtyId} (eliminarEspecialidad)
        [HttpDelete("specialties/{specialtyId}")]
        public async Task<ActionResult> DeleteSpecialty([FromRoute] int specialtyId)
        {
            // El middleware manejará la excepción si la especialidad no existe
            await _adminsService.DeleteSpecialtyAsync(specialtyId);
            return NoContent();
        }

        // DELETE /admin/appointments/{appointmentId} (eliminarTurno)
        [HttpDelete("appointments/{appointmentId}")]
        public async Task<ActionResult> DeleteAppointment([FromRoute] int appointmentId)
        {
            // El middleware manejará la excepción si el turno no existe
            await _adminsService.DeleteAppointmentAsync(appointmentId);
            return NoContent();
        }

        // DELETE /admin/insurance/{insuranceId} (eliminarObraSocial)
        [HttpDelete("insurance/{insuranceId}")]
        public async Task<ActionResult> DeleteInsurance([FromRoute] int insuranceId)
        {
            // El middleware manejará la excepción si la obra social no existe
            await _adminsService.DeleteInsuranceAsync(insuranceId);
            return NoContent();
        }
    }
}