using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfessionalController : ControllerBase
    {
        private readonly IProfessionalService _professionalsService;

        public ProfessionalController(IProfessionalService professionalsService)
        {
            _professionalsService = professionalsService;
        }

        // POST /Professional (Crear Profesional) - Solo admin
        [HttpPost]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult<ProfessionalDTO>> CreateProfessional([FromBody] CreationProfessionalDTO dto)
        {
            // El middleware manejará las excepciones de validación o si la especialidad no existe
            var created = await _professionalsService.CreateProfessionalAsync(dto);

            return CreatedAtAction(nameof(UserController.GetById), "User", new { id = created.Id }, created);
        }

        // GET /Professional/{professionalId}/appointments (verTurnos)
        [HttpGet("{professionalId}/appointments")]
        [Authorize(Roles = "Professional,Administrator")] // ⬅ Solo profesional o admin
        public async Task<ActionResult<IEnumerable<AppointmentDTO>>> ViewAppointment([FromRoute] int professionalId)
        {
            // Verificar que el usuario autenticado sea el profesional o un administrador
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != professionalId)
            {
                return Forbid(); // 403 Forbidden
            }

            // El middleware manejará la excepción si el profesional no existe
            var appointments = await _professionalsService.ViewAppointmentAsync(professionalId);
            return Ok(appointments);
        }

        // POST /Professional/appointments/{appointmentId}/accept (aceptarTurno)
        [HttpPost("appointments/{appointmentId}/accept")]
        [Authorize(Roles = "Professional")] // ⬅ Solo profesionales
        public async Task<ActionResult> AcceptAppointment([FromRoute] int appointmentId)
        {
            // ✅ Obtener ID del profesional desde el token JWT
            int professionalId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (professionalId == 0)
            {
                return Unauthorized("No se pudo identificar al profesional.");
            }

            // El middleware manejará las excepciones si el turno no existe o no pertenece al profesional
            await _professionalsService.AcceptAppointmentAsync(professionalId, appointmentId);
            return NoContent();
        }

        // POST /Professional/appointments/{appointmentId}/reject (rechazarTurno)
        [HttpPost("appointments/{appointmentId}/reject")]
        [Authorize(Roles = "Professional")] // ⬅ Solo profesionales
        public async Task<ActionResult> RejectAppointment([FromRoute] int appointmentId)
        {
            // ✅ Obtener ID del profesional desde el token JWT
            int professionalId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            if (professionalId == 0)
            {
                return Unauthorized("No se pudo identificar al profesional.");
            }

            // El middleware manejará las excepciones si el turno no existe o no pertenece al profesional
            await _professionalsService.RejectAppointmentAsync(professionalId, appointmentId);
            return NoContent();
        }

        // GET /Professional/{professionalId}/patients (listarPacientes)
        [HttpGet("{professionalId}/patients")]
        [Authorize(Roles = "Professional,Administrator")] // ⬅ Solo profesional o admin
        public async Task<ActionResult<IEnumerable<PatientDTO>>> ListPatient([FromRoute] int professionalId)
        {
            // Verificar que el usuario autenticado sea el profesional o un administrador
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Administrator" && userId != professionalId)
            {
                return Forbid(); // 403 Forbidden
            }

            // El middleware manejará la excepción si el profesional no existe
            var patients = await _professionalsService.ListPatientAsync(professionalId);
            return Ok(patients);
        }
    }
}