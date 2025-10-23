using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessionalsController : ControllerBase
    {
        private readonly IProfessionalsService _professionalsService;
        private readonly ApplicationDbContext _context;

        public ProfessionalsController(IProfessionalsService professionalsService, ApplicationDbContext context)
        {
            _professionalsService = professionalsService;
            _context = context;
        }

        // POST /Professionals (Crear Profesional)
        [HttpPost]
        public async Task<ActionResult<ProfessionalsDTO>> CreateProfessional([FromBody] CreationProfessionalsDTO dto)
        {
            // Primero validás que el ID sea válido
            if (dto.SpecialtyId <= 0)
            {
                return BadRequest("Debe especificar un ID de especialidad válido.");
            }

            // Luego verificás si la especialidad existe en la base de datos
            var specialtyExists = await _context.Specialties.AnyAsync(s => s.Id == dto.SpecialtyId);
            if (!specialtyExists)
            {
                return NotFound($"No existe una especialidad con el ID {dto.SpecialtyId}.");
            }

            var created = await _professionalsService.CreateProfessionalAsync(dto);

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
            int professionalId = 1; // TODO: Obtener del token JWT
            bool success = await _professionalsService.AcceptAppointmentAsync(professionalId, appointmentId);
            return success ? NoContent() : BadRequest("Cannot accept appointment.");
        }

        // POST /Professionals/appointments/{appointmentId}/reject (rechazarTurno)
        [HttpPost("appointments/{appointmentId}/reject")]
        public async Task<ActionResult> RejectAppointment([FromRoute] int appointmentId)
        {
            int professionalId = 1; // TODO: Obtener del token JWT
            bool success = await _professionalsService.RejectAppointmentAsync(professionalId, appointmentId);
            return success ? NoContent() : BadRequest("Cannot reject appointment.");
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
