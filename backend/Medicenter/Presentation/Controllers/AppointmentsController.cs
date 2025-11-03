using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // ⬅️ Todos los endpoints requieren autenticación
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentsService;

        public AppointmentsController(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        // GET /Appointments - Ver todos los turnos (solo admin)
        [HttpGet]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<IEnumerable<AppointmentsDTO>>> GetAll()
        {
            var appointments = await _appointmentsService.GetAllAsync();
            return Ok(appointments);
        }

        // GET /Appointments/{id} - Ver un turno específico
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator,Professional,Patient")] // ⬅️ Cualquier usuario autenticado
        public async Task<ActionResult<AppointmentsDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var appointment = await _appointmentsService.GetByIdAsync(id);
                return Ok(appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Appointments (asignarTurno) - Solo admin y profesionales puede asignar directamente
        [HttpPost]
        [Authorize(Roles = "Administrator, Professional")] // ⬅️ Solo administradores y profesionales
        public async Task<ActionResult<AppointmentsDTO>> AssignAppointment([FromBody] CreationAppointmentDTO dto)
        {
            try
            {
                var created = await _appointmentsService.AssignAppointmentAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
                catch (ArgumentException ex)
    {
        // ⬅️ Esta excepción ocurre cuando la fecha es feriado o inválida
        return BadRequest(ex.Message);
    }
        }

        // PUT /Appointments/{id} (modificarTurno) - Solo admin
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<AppointmentsDTO>> UpdateAppointment([FromRoute] int id, [FromBody] CreationAppointmentDTO dto)
        {
            try
            {
                var updated = await _appointmentsService.UpdateAppointmentAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Appointments/{id}/confirm (confirmarTurno) - Admin o el profesional asignado
        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "Administrator,Professional")] // ⬅️ Admin o profesional
        public async Task<ActionResult<AppointmentsDTO>> ConfirmAppointment([FromRoute] int id)
        {
            try
            {
                var confirmed = await _appointmentsService.ConfirmAppointmentAsync(id);
                return Ok(confirmed);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Appointments/{id} (cancelarTurno) - Admin o los involucrados
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,Professional,Patient")] // ⬅️ Cualquiera involucrado
        public async Task<ActionResult> CancelAppointment([FromRoute] int id)
        {
            try
            {
                await _appointmentsService.CancelAppointmentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}