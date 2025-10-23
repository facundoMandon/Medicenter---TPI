using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidaysController : ControllerBase
    {
        private readonly IHolidaysService _holidaysService;

        public HolidaysController(IHolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        // GET: api/holidays?country=US&year=2025
        [HttpGet]
        public async Task<IActionResult> GetHolidays(
    [FromQuery] string country = "US",
    [FromQuery] string year = "2025",
    [FromQuery] string? month = null,
    [FromQuery] string? day = null)
        {
            var holidaysJson = await _holidaysService.GetHolidaysAsync(country, year, month, day);
            return Content(holidaysJson, "application/json");
        }

    }
}
