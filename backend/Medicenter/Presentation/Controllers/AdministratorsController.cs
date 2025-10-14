using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Models.Request;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministratorsController : ControllerBase
    {
        // Asegúrate de definir IAdministratorsService y CreationAdministratorsDTO en tus capas correspondientes.
        private readonly IAdministratorsService _service;

        public AdministratorsController(IAdministratorsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreationAdministratorsDTO dto)
        {
            // Asumiendo que el resultado de la creación tiene una propiedad 'Id'
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreationAdministratorsDTO dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Deberías manejar la lógica para verificar si existe y si se borró
            await _service.DeleteAsync(id);
            return NoContent(); // 204 No Content para borrado exitoso
        }
    }
}