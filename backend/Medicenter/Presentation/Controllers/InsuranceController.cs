using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")] // Ruta: /Insurance
    // Asumimos que la mayoría de los endpoints son para el Administrador
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        // 1. GET /Insurance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InsuranceDTO>>> GetAll()
        {
            var insurances = await _insuranceService.GetAllAsync();
            return Ok(insurances);
        }

        // 2. POST /Insurance (Crear Obra Social)
        [HttpPost]
        public async Task<ActionResult<InsuranceDTO>> CreateInsurance([FromBody] CreationInsuranceDTO dto)
        {
            var created = await _insuranceService.CreateInsuranceAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // 3. GET /Insurance/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InsuranceDTO>> GetById([FromRoute] int id)
        {
            try
            {
                var insurance = await _insuranceService.GetByIdAsync(id);
                return Ok(insurance);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // 4. DELETE /Insurance/{id} (EliminarObraSocial)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteInsurance([FromRoute] int id)
        {
            try
            {
                await _insuranceService.DeleteInsuranceAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // --- MÉTODOS DE GESTIÓN DE AFILIADOS ---

        // 5. POST /Insurance/{insuranceId}/affiliates/{patientId} (añadirAfiliado)
        [HttpPost("{insuranceId}/affiliates/{patientId}")]
        public async Task<ActionResult> AddAffiliate([FromRoute] int insuranceId, [FromRoute] int patientId)
        {
            await _insuranceService.AddAffiliateAsync(insuranceId, patientId);
            return NoContent();
        }

        // 6. PUT /Insurance/affiliates/{patientId}/coverage (cambiarCobertura)
        [HttpPut("affiliates/{patientId}/coverage")]
        public async Task<ActionResult> ChangeCoverage([FromRoute] int patientId, [FromBody] string newPlan)
        {
            await _insuranceService.ChangeCoverageAsync(patientId, newPlan);
            return NoContent();
        }
    }
}