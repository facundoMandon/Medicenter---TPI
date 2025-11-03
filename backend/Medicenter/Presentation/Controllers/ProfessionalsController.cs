using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        // POST /Professionals (Crear Profesional) - Solo admin
        [HttpPost]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<ProfessionalsDTO>> CreateProfessional([FromBody] CreationProfessionalsDTO dto)
        {
            if (dto.SpecialtyId <= 0)
            {
                return BadRequest("Debe especificar un ID de especialidad válido.");
            }

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
        [Authorize(Roles = "Professional,Administrator")] // ⬅️ Solo profesional o admin
        public async Task<ActionResult<IEnumerable<AppointmentsDTO>>> ViewAppointments([FromRoute] int professionalId)
        {
            // Verificar que el usuario autenticado sea el profesional o un administrador
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != professionalId)
            {
                return Forbid(); // 403 Forbidden
            }

            var appointments = await _professionalsService.ViewAppointmentsAsync(professionalId);
            return Ok(appointments);
        }

        // POST /Professionals/appointments/{appointmentId}/accept (aceptarTurno)
        [HttpPost("appointments/{appointmentId}/accept")]
        [Authorize(Roles = "Professional")] // ⬅️ Solo profesionales
        public async Task<ActionResult> AcceptAppointment([FromRoute] int appointmentId)
        {
            // ✅ Obtener ID del profesional desde el token JWT
            int professionalId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (professionalId == 0)
            {
                return Unauthorized("No se pudo identificar al profesional.");
            }

            bool success = await _professionalsService.AcceptAppointmentAsync(professionalId, appointmentId);
            return success ? NoContent() : BadRequest("Cannot accept appointment.");
        }

        // POST /Professionals/appointments/{appointmentId}/reject (rechazarTurno)
        [HttpPost("appointments/{appointmentId}/reject")]
        [Authorize(Roles = "Professional")] // ⬅️ Solo profesionales
        public async Task<ActionResult> RejectAppointment([FromRoute] int appointmentId)
        {
            // ✅ Obtener ID del profesional desde el token JWT
            int professionalId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (professionalId == 0)
            {
                return Unauthorized("No se pudo identificar al profesional.");
            }

            bool success = await _professionalsService.RejectAppointmentAsync(professionalId, appointmentId);
            return success ? NoContent() : BadRequest("Cannot reject appointment.");
        }

        // GET /Professionals/{professionalId}/patients (listarPacientes)
        [HttpGet("{professionalId}/patients")]
        [Authorize(Roles = "Professional,Administrator")] // ⬅️ Solo profesional o admin
        public async Task<ActionResult<IEnumerable<PatientsDTO>>> ListPatients([FromRoute] int professionalId)
        {
            // Verificar que el usuario autenticado sea el profesional o un administrador
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != professionalId)
            {
                return Forbid(); // 403 Forbidden
            }

            var patients = await _professionalsService.ListPatientsAsync(professionalId);
            return Ok(patients);
        }
    }
}