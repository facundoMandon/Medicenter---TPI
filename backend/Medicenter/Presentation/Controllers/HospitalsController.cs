using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalsService _hospitalsService;

        public HospitalsController(IHospitalsService hospitalsService)
        {
            _hospitalsService = hospitalsService;
        }

        // GET /Hospitals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalsDTO>>> GetAll()
        {
            var hospitals = await _hospitalsService.GetAllAsync();
            return Ok(hospitals);
        }

        // GET /Hospitals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalsDTO>> GetById([FromRoute] int id)
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

        // POST /Hospitals
        [HttpPost]
        public async Task<ActionResult<HospitalsDTO>> CreateHospital([FromBody] CreationHospitalsDTO dto)
        {
            var created = await _hospitalsService.CreateHospitalAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /Hospitals/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<HospitalsDTO>> UpdateHospital([FromRoute] int id, [FromBody] CreationHospitalsDTO dto)
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

        // DELETE /Hospitals/{id}
        [HttpDelete("{id}")]
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

        // POST /Hospitals/{hospitalId}/professionals/{professionalId} (registrarProfesional)
        [HttpPost("{hospitalId}/professionals/{professionalId}")]
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

        // GET /Hospitals/{hospitalId}/professionals (listarProfesionales)
        [HttpGet("{hospitalId}/professionals")]
        public async Task<ActionResult<IEnumerable<ProfessionalsDTO>>> ListProfessionals([FromRoute] int hospitalId)
        {
            var professionals = await _hospitalsService.ListProfessionalsAsync(hospitalId);
            return Ok(professionals);
        }

        // DELETE /Hospitals/{hospitalId}/professionals/{professionalId} (eliminarProfesionales)
        [HttpDelete("{hospitalId}/professionals/{professionalId}")]
        public async Task<ActionResult> RemoveProfessional([FromRoute] int hospitalId, [FromRoute] int professionalId)
        {
            await _hospitalsService.RemoveProfessionalAsync(hospitalId, professionalId);
            return NoContent();
        }
    }
}