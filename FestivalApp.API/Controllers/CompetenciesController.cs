using FestivalAppAPI.DTOs.Competency;
using FestivalAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FestivalAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetenciesController : ControllerBase
    {
        private readonly CompetencyService _competencyService;

        public CompetenciesController(CompetencyService competencyService)
        {
            _competencyService = competencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var competencies = await _competencyService.GetAllAsync();
            return Ok(competencies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var competency = await _competencyService.GetByIdAsync(id);
            if (competency == null) return NotFound();
            return Ok(competency);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateCompetencyDto dto)
        {
            var competency = await _competencyService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = competency.Id }, competency);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCompetencyDto dto)
        {
            var competency = await _competencyService.UpdateAsync(id, dto);
            if (competency == null) return NotFound();
            return Ok(competency);
        }
    }
}