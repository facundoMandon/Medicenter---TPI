using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _hospitalsService;

        public HospitalController(IHospitalService hospitalsService)
        {
            _hospitalsService = hospitalsService;
        }

        // GET /Hospital - Ver todos los hospitales (público para que pacientes vean opciones)
        [HttpGet]
        [AllowAnonymous] // ⬅️ Público - pacientes necesitan ver hospitales disponibles
        public async Task<ActionResult<IEnumerable<HospitalDTO>>> GetAll()
        {
            var hospitals = await _hospitalsService.GetAllAsync();
            return Ok(hospitals);
        }

        // GET /Hospital/{id} - Ver un hospital (público)
        [HttpGet("{id}")]
        [AllowAnonymous] // ⬅️ Público
        public async Task<ActionResult<HospitalDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var hospital = await _hospitalsService.GetByIdAsync(id);
                return Ok(hospital);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Hospital - Crear hospital (solo admin)
        [HttpPost]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<HospitalDTO>> CreateHospital([FromBody] CreationHospitalDTO dto)
        {
            var created = await _hospitalsService.CreateHospitalAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /Hospital/{id} - Actualizar hospital (solo admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult<HospitalDTO>> UpdateHospital([FromRoute] int id, [FromBody] CreationHospitalDTO dto)
        {
            try
            {
                var updated = await _hospitalsService.UpdateHospitalAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Hospital/{id} - Eliminar hospital (solo admin)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult> DeleteHospital([FromRoute] int id)
        {
            try
            {
                await _hospitalsService.DeleteHospitalAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Hospital/{hospitalId}/professionals/{professionalId} (registrarProfesional)
        [HttpPost("{hospitalId}/professionals/{professionalId}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult> RegisterProfessional([FromRoute] int hospitalId, [FromRoute] int professionalId)
        {
            try
            {
                await _hospitalsService.RegisterProfessionalAsync(hospitalId, professionalId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET /Hospital/{hospitalId}/professionals (listarProfesionales)
        [HttpGet("{hospitalId}/professionals")]
        [AllowAnonymous] // ⬅️ Público - pacientes necesitan ver qué profesionales atienden dónde
        public async Task<ActionResult<IEnumerable<ProfessionalDTO>>> ListProfessional([FromRoute] int hospitalId)
        {
            var professionals = await _hospitalsService.ListProfessionalAsync(hospitalId);
            return Ok(professionals);
        }

        // DELETE /Hospital/{hospitalId}/professionals/{professionalId} (eliminarProfesionales)
        [HttpDelete("{hospitalId}/professionals/{professionalId}")]
        [Authorize(Roles = "Administrator")] // ⬅️ Solo administradores
        public async Task<ActionResult> RemoveProfessional([FromRoute] int hospitalId, [FromRoute] int professionalId)
        {
            await _hospitalsService.RemoveProfessionalAsync(hospitalId, professionalId);
            return NoContent();
        }
    }
}