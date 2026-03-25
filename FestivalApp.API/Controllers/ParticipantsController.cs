using FestivalAppAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FestivalAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantsController : ControllerBase
    {
        private readonly ParticipantService _participantService;

        public ParticipantsController(ParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? name,
            [FromQuery] int? competencyId,
            [FromQuery] string? category,
            [FromQuery] int? regionId)
        {
            var participants = await _participantService.GetParticipantsAsync(name, competencyId, category, regionId);
            return Ok(participants);
        }
    }
}