using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Enums;
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

        // GET /Insurance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsuranceDTO>>> GetAll()
        {
            var insurances = await _insuranceService.GetAllAsync();
            return Ok(insurances);
        }

        // GET /Insurance/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InsuranceDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var insurance = await _insuranceService.GetByIdAsync(id);
                return Ok(insurance);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Insurance (Crear Obra Social)
        [HttpPost]
        public async Task<ActionResult<InsuranceDTO>> CreateInsurance([FromBody] CreationInsuranceDTO dto)
        {
            var created = await _insuranceService.CreateInsuranceAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /Insurance/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<InsuranceDTO>> UpdateInsurance([FromRoute] int id, [FromBody] CreationInsuranceDTO dto)
        {
            try
            {
                var updated = await _insuranceService.UpdateInsuranceAsync(id, dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Insurance/{id} (EliminarObraSocial)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInsurance([FromRoute] int id)
        {
            try
            {
                await _insuranceService.DeleteInsuranceAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /Insurance/{insuranceId}/affiliates/{patientId} (añadirAfiliado)
        [HttpPost("{insuranceId}/affiliates/{patientId}")]
        public async Task<ActionResult> AddAffiliate([FromRoute] int insuranceId, [FromRoute] int patientId)
        {
            try
            {
                await _insuranceService.AddAffiliateAsync(insuranceId, patientId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /Insurance/{insuranceId}/affiliates/{patientId} (eliminarAfiliado)
        [HttpDelete("{insuranceId}/affiliates/{patientId}")]
        public async Task<ActionResult> RemoveAffiliate([FromRoute] int insuranceId, [FromRoute] int patientId)
        {
            try
            {
                await _insuranceService.RemoveAffiliateAsync(insuranceId, patientId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT /Insurance/affiliates/{patientId}/coverage (cambiarCobertura)
        [HttpPut("affiliates/{patientId}/coverage")]
        public async Task<ActionResult> ChangeCoverage([FromRoute] int patientId, [FromBody] MedicalCoverageType newCoverage)
        {
            try
            {
                await _insuranceService.ChangeCoverageAsync(patientId, newCoverage);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
