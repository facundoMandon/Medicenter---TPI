using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // ⬅ Todos los endpoints requieren autenticación
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentsService;

        public AppointmentController(IAppointmentService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        // GET /Appointment - Ver todos los turnos (solo admin)
        [HttpGet]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> GetAll()
        {
            var appointments = await _appointmentsService.GetAllAsync();
            return Ok(appointments);
        }

        // GET /Appointment/{id} - Ver un turno específico
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Professional,Patient")] // ⬅ Cualquier usuario autenticado
        public async Task<ActionResult<AppointmentDTO>> GetById([FromRoute] int id)
        {
            // El middleware manejará la excepción si el turno no existe
            var appointment = await _appointmentsService.GetByIdAsync(id);
            return Ok(appointment);
        }

        // POST /Appointment (asignarTurno) - Solo admin y profesionales puede asignar directamente
        [HttpPost]
        [Authorize(Roles = "Administrator, Professional")] // ⬅ Solo administradores y profesionales
        public async Task<ActionResult<AppointmentDTO>> AssignAppointment([FromBody] CreationAppointmentDTO dto)
        {
            // El middleware manejará las excepciones si:
            // - El paciente o profesional no existen (NotFoundException -> 404)
            // - Los datos son inválidos (ValidationException -> 400)
            // - La fecha es feriado o inválida (ValidationException -> 400)
            var created = await _appointmentsService.AssignAppointmentAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /Appointment/{id} (modificarTurno) - Solo admin
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult<AppointmentDTO>> UpdateAppointment([FromRoute] int id, [FromBody] CreationAppointmentDTO dto)
        {
            // El middleware manejará las excepciones de validación o si el turno no existe
            var updated = await _appointmentsService.UpdateAppointmentAsync(id, dto);
            return Ok(updated);
        }

        // POST /Appointment/{id}/confirm (confirmarTurno) - Admin o el profesional asignado
        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Administrator,Professional")] // ⬅ Admin o profesional
        public async Task<ActionResult<AppointmentDTO>> ConfirmAppointment([FromRoute] int id)
        {
            // El middleware manejará la excepción si el turno no existe
            var confirmed = await _appointmentsService.ConfirmAppointmentAsync(id);
            return Ok(confirmed);
        }

        // DELETE /Appointment/{id} (cancelarTurno) - Admin o los involucrados
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Professional,Patient")] // ⬅ Cualquiera involucrado
        public async Task<ActionResult> CancelAppointment([FromRoute] int id)
        {
            // El middleware manejará la excepción si el turno no existe
            await _appointmentsService.CancelAppointmentAsync(id);
            return NoContent();
        }
    }
}