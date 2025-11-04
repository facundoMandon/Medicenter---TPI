using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientsService;

        public PatientController(IPatientService patientsService)
        {
            _patientsService = patientsService;
        }

        // POST /Patient (Crear Paciente) - Registro público
        [HttpPost]
        [AllowAnonymous] // ⬅️ Cualquiera puede registrarse como paciente
        public async Task<ActionResult<PatientDTO>> CreatePatient([FromBody] CreationPatientDTO dto)
        {
            var created = await _patientsService.CreatePatientAsync(dto);
            return CreatedAtAction(nameof(UserController.GetById), "User", new { id = created.Id }, created);
        }

        // GET /Patient/{patientId}/appointments (verTurnos)
        [HttpGet("{patientId}/appointments")]
        [Authorize(Roles = "Patient,Administrator")] // ⬅️ Solo paciente o admin
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> ViewAppointment([FromRoute] int patientId)
        {
            // Verificar que el usuario autenticado sea el paciente o un administrador
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != patientId)
            {
                return Forbid(); // 403 Forbidden
            }

            var appointments = await _patientsService.ViewAppointmentAsync(patientId);
            return Ok(appointments);
        }

        // POST /Patient/{patientId}/appointments (pedirTurno)
        [HttpPost("{patientId}/appointments")]
        [Authorize(Roles = "Patient")] // ⬅️ Solo pacientes
        public async Task<ActionResult<AppointmentDTO>> RequestAppointment([FromRoute] int patientId, [FromBody] AppointmentRequestDTO request)
        {
            // Verificar que el usuario autenticado sea el paciente
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userId != patientId)
            {
                return Forbid(); // 403 Forbidden
            }

            try
            {
                var newAppointment = await _patientsService.RequestAppointmentAsync(patientId, request);
                return CreatedAtAction(nameof(ViewAppointment), new { patientId = patientId }, newAppointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Patient/{patientId}/appointments/{appointmentId} (cancelarTurno)
        [HttpDelete("{patientId}/appointments/{appointmentId}")]
        [Authorize(Roles = "Patient")] // ⬅️ Solo pacientes
        public async Task<ActionResult> CancelAppointment([FromRoute] int patientId, [FromRoute] int appointmentId)
        {
            // Verificar que el usuario autenticado sea el paciente
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (userId != patientId)
            {
                return Forbid(); // 403 Forbidden
            }

            try
            {
                await _patientsService.CancelAppointmentAsync(patientId, appointmentId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }
    }
}