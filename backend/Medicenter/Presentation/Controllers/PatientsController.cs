using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")] // Ruta: /Patients
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsService _patientsService;

        public PatientsController(IPatientsService patientsService)
        {
            _patientsService = patientsService;
        }

        // POST /Patients (Crear Paciente)
        [HttpPost]
        public async Task<ActionResult<PatientsDTO>> CreatePatient([FromBody] CreationPatientsDTO dto)
        {
            var created = await _patientsService.CreatePatientAsync(dto);
            // Redirige al método GetById del controlador Users
            return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = created.Id }, created);
        }

        // GET /Patients/{patientId}/appointments (verTurnos)
        [HttpGet("{patientId}/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentsDTO>>> ViewAppointments([FromRoute] int patientId)
        {
            var appointments = await _patientsService.ViewAppointmentsAsync(patientId);
            return Ok(appointments);
        }

        // POST /Patients/{patientId}/appointments (pedirTurno)
        [HttpPost("{patientId}/appointments")]
        public async Task<ActionResult<AppointmentsDTO>> RequestAppointment([FromRoute] int patientId, [FromBody] AppointmentRequestDTO request)
        {
            var newAppointment = await _patientsService.RequestAppointmentAsync(patientId, request);
            return CreatedAtAction(nameof(ViewAppointments), new { patientId = patientId }, newAppointment);
        }

        // DELETE /Patients/{patientId}/appointments/{appointmentId} (cancelarTurno)
        [HttpDelete("{patientId}/appointments/{appointmentId}")]
        public async Task<ActionResult> CancelAppointment([FromRoute] int patientId, [FromRoute] int appointmentId)
        {
            await _patientsService.CancelAppointmentAsync(patientId, appointmentId);
            return NoContent();
        }
    }
}