using FestivalAppAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace FestivalAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfigController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("festival-date")]
        public async Task<IActionResult> GetFestivalDate()
        {
            var config = await _context.SystemConfigs.FindAsync("FestivalStartDate");
            if (config == null)
                return Ok(new { festivalStartDate = (DateTime?)null });
            return Ok(new { festivalStartDate = config.Value });
        }
    }
}