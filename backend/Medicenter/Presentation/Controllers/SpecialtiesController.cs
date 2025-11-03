using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtiesService _specialtiesService;

        public SpecialtiesController(ISpecialtiesService specialtiesService)
        {
            _specialtiesService = specialtiesService;
        }

        // GET /Specialties - Ver todas las especialidades (público)
        [HttpGet]
        [AllowAnonymous] // ⬅️ Público - pacientes necesitan ver especialidades disponibles
        public async Task<ActionResult<IEnumerable<SpecialtiesDTO>>> GetAll()
        {
            var specialties = await _specialtiesService.GetAllAsync();
            return Ok(specialties);
        }

        // GET /Specialties/{id} - Ver una especialidad (público)
        [HttpGet("{id}")]
        [AllowAnonymous] // ⬅️ Público
        public async Task<ActionResult<SpecialtiesDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var specialty = await _specialtiesService.GetByIdAsync(id);
                return Ok(specialty);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Specialties (añadirEspecialidad) - Solo admin
        [HttpPost]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<SpecialtiesDTO>> CreateSpecialty([FromBody] CreationSpecialtiesDTO dto)
        {
            var created = await _specialtiesService.CreateSpecialtyAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /Specialties/{id} (modificarEspecialidad) - Solo admin
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<SpecialtiesDTO>> UpdateSpecialty([FromRoute] int id, [FromBody] CreationSpecialtiesDTO dto)
        {
            try
            {
                var updated = await _specialtiesService.UpdateSpecialtyAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Specialties/{id} (eliminarEspecialidad) - Solo admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult> DeleteSpecialty([FromRoute] int id)
        {
            try
            {
                await _specialtiesService.DeleteSpecialtyAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Specialties/{specialtyId}/assign/{professionalId} (asignarEspecialidad) - Solo admin
        [HttpPost("{specialtyId}/assign/{professionalId}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult> AssignSpecialtyToProfessional([FromRoute] int specialtyId, [FromRoute] int professionalId)
        {
            try
            {
                await _specialtiesService.AssignSpecialtyToProfessionalAsync(specialtyId, professionalId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Specialties/{specialtyId}/remove/{professionalId} (quitarEspecialidad) - Solo admin
        [HttpDelete("{specialtyId}/remove/{professionalId}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult> RemoveSpecialtyFromProfessional([FromRoute] int specialtyId, [FromRoute] int professionalId)
        {
            try
            {
                await _specialtiesService.RemoveSpecialtyFromProfessionalAsync(specialtyId, professionalId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}