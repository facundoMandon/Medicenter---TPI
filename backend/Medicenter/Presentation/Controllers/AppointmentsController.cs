using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentsService _appointmentsService;

        public AppointmentsController(IAppointmentsService appointmentsService)
        {
            _appointmentsService = appointmentsService;
        }

        // GET /Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentsDTO>>> GetAll()
        {
            var appointments = await _appointmentsService.GetAllAsync();
            return Ok(appointments);
        }

        // GET /Appointments/{id}
        [HttpGet("{id}")]
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

        // POST /Appointments (asignarTurno)
        [HttpPost]
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
        }

        // PUT /Appointments/{id} (modificarTurno)
        [HttpPut("{id}")]
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

        // POST /Appointments/{id}/confirm (confirmarTurno)
        [HttpPost("{id}/confirm")]
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

        // DELETE /Appointments/{id} (cancelarTurno)
        [HttpDelete("{id}")]
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
