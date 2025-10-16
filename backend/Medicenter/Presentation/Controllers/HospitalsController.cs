using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")] // Ruta base: /Hospitals
    // Nota: Se asume que este controlador estaría protegido con autorización para el rol de Administrador.
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalsService _hospitalsService;

        public HospitalsController(IHospitalsService hospitalsService)
        {
            _hospitalsService = hospitalsService;
        }

        // 1. GET /Hospitals (Listar todos los hospitales)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalsDTO>>> GetAll()
        {
            var hospitals = await _hospitalsService.GetAllAsync();
            return Ok(hospitals);
        }

        // 2. GET /Hospitals/{id} (Obtener hospital por ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalsDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var hospital = await _hospitalsService.GetByIdAsync(id);
                return Ok(hospital);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // 3. POST /Hospitals (Crear un nuevo hospital)
        [HttpPost]
        public async Task<ActionResult<HospitalsDTO>> CreateHospital([FromBody] CreationHospitalsDTO dto)
        {
            var created = await _hospitalsService.CreateHospitalAsync(dto);
            // Devuelve 201 Created y la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // 4. PUT /Hospitals/{id} (Modificar hospital)
        [HttpPut("{id}")]
        public async Task<ActionResult<HospitalsDTO>> UpdateHospital([FromRoute] int id, [FromBody] CreationHospitalsDTO dto)
        {
            try
            {
                var updated = await _hospitalsService.UpdateHospitalAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // 5. DELETE /Hospitals/{id} (Eliminar hospital)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHospital([FromRoute] int id)
        {
            try
            {
                await _hospitalsService.DeleteHospitalAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // --- MÉTODOS DE GESTIÓN DE PROFESIONALES (Diagrama HOSPITALES) ---

        // 6. POST /Hospitals/{hospitalId}/professionals/{professionalId} (registrarProfesional)
        [HttpPost("{hospitalId}/professionals/{professionalId}")]
        public async Task<ActionResult> RegisterProfessional([FromRoute] int hospitalId, [FromRoute] int professionalId)
        {
            try
            {
                await _hospitalsService.RegisterProfessionalAsync(hospitalId, professionalId);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            // NOTA: Para editarProfesional, se asume que el administrador usa el endpoint PUT de UsersController.
        }

        // 7. GET /Hospitals/{hospitalId}/professionals (listarProfesionales)
        [HttpGet("{hospitalId}/professionals")]
        public async Task<ActionResult<IEnumerable<ProfessionalsDTO>>> ListProfessionals([FromRoute] int hospitalId)
        {
            try
            {
                var professionals = await _hospitalsService.ListProfessionalsAsync(hospitalId);
                return Ok(professionals);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Hospital ID {hospitalId} not found.");
            }
        }

        // 8. DELETE /Hospitals/{hospitalId}/professionals/{professionalId} (eliminarProfesionales)
        [HttpDelete("{hospitalId}/professionals/{professionalId}")]
        public async Task<ActionResult> RemoveProfessional([FromRoute] int hospitalId, [FromRoute] int professionalId)
        {
            try
            {
                await _hospitalsService.RemoveProfessionalAsync(hospitalId, professionalId);
                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}