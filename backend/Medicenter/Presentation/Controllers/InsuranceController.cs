using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        // GET /Insurance - Ver todas las obras sociales (público)
        [HttpGet]
        [AllowAnonymous] // ⬅ Público - pacientes necesitan ver obras sociales al registrarse
        public async Task<ActionResult<IEnumerable<InsuranceDTO>>> GetAll()
        {
            var insurances = await _insuranceService.GetAllAsync();
            return Ok(insurances);
        }

        // GET /Insurance/{id} - Ver una obra social (público)
        [HttpGet("{id}")]
        [AllowAnonymous] // ⬅ Público
        public async Task<ActionResult<InsuranceDTO>> GetById([FromRoute] int id)
        {
            // El middleware manejará la excepción si la obra social no existe
            var insurance = await _insuranceService.GetByIdAsync(id);
            return Ok(insurance);
        }

        // POST /Insurance (Crear Obra Social) - Solo admin
        [HttpPost]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult<InsuranceDTO>> CreateInsurance([FromBody] CreationInsuranceDTO dto)
        {
            // El middleware manejará las excepciones de validación
            var created = await _insuranceService.CreateInsuranceAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /Insurance/{id} - Actualizar obra social (solo admin)
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult<InsuranceDTO>> UpdateInsurance([FromRoute] int id, [FromBody] CreationInsuranceDTO dto)
        {
            // El middleware manejará las excepciones de validación o si la obra social no existe
            var updated = await _insuranceService.UpdateInsuranceAsync(id, dto);
            return Ok(updated);
        }

        // DELETE /Insurance/{id} (EliminarObraSocial) - Solo admin
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")] // ⬅ Solo administradores
        public async Task<ActionResult> DeleteInsurance([FromRoute] int id)
        {
            // El middleware manejará la excepción si la obra social no existe
            await _insuranceService.DeleteInsuranceAsync(id);
            return NoContent();
        }

        // PUT /Insurance/affiliates/{patientId}/coverage (cambiarCobertura) - Admin o el paciente
        [HttpPut("affiliates/{patientId}/coverage")]
        [Authorize(Roles = "Administrator,Patient")] // ⬅ Admin o el paciente puede cambiar su cobertura
        public async Task<ActionResult> ChangeCoverage([FromRoute] int patientId, [FromBody] MedicalCoverageType newCoverage)
        {
            // El middleware manejará la excepción si el paciente no existe
            await _insuranceService.ChangeCoverageAsync(patientId, newCoverage);
            return NoContent();
        }
    }
}