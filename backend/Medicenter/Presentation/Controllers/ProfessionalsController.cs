using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")] // Ruta: /Professionals
    public class ProfessionalsController : ControllerBase
    {
        private readonly IProfessionalsService _professionalsService;

        public ProfessionalsController(IProfessionalsService professionalsService)
        {
            _professionalsService = professionalsService;
        }

        // POST /Professionals (Crear Profesional)
        [HttpPost]
        public async Task<ActionResult<ProfessionalsDTO>> CreateProfessional([FromBody] CreationProfessionalsDTO dto)
        {
            var created = await _professionalsService.CreateProfessionalAsync(dto);
            // Redirige al método GetById del controlador Users
            return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = created.Id }, created);
        }

        // GET /Professionals/{professionalId}/appointments (verTurnos)
        [HttpGet("{professionalId}/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentsDTO>>> ViewAppointments([FromRoute] int professionalId)
        {
            var appointments = await _professionalsService.ViewAppointmentsAsync(professionalId);
            return Ok(appointments);
        }

        // POST /Professionals/appointments/{appointmentId}/accept (aceptarTurno)
        [HttpPost("appointments/{appointmentId}/accept")]
        public async Task<ActionResult> AcceptAppointment([FromRoute] int appointmentId)
        {
            // En una aplicación real, el ID del profesional se obtiene del token de autenticación.
            int professionalId = 1; // ID simulado
            bool success = await _professionalsService.AcceptAppointmentAsync(professionalId, appointmentId);
            return success ? NoContent() : BadRequest("Cannot accept appointment or appointment not found.");
        }

        // GET /Professionals/{professionalId}/patients (listarPacientes)
        [HttpGet("{professionalId}/patients")]
        public async Task<ActionResult<IEnumerable<PatientsDTO>>> ListPatients([FromRoute] int professionalId)
        {
            var patients = await _professionalsService.ListPatientsAsync(professionalId);
            return Ok(patients);
        }
    }
}
