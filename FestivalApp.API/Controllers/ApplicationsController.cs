using System.Security.Claims;
using FestivalAppAPI.DTOs.Application;
using FestivalAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FestivalAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationService _applicationService;

        public ApplicationsController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        public async Task<IActionResult> Create(CreateApplicationDto dto)
        {
            try
            {
                var app = await _applicationService.CreateAsync(GetUserId(), dto);
                return Ok(app);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var apps = await _applicationService.GetUserApplicationsAsync(GetUserId());
            return Ok(apps);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var apps = await _applicationService.GetAllApplicationsAsync();
            return Ok(apps);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateApplicationStatusDto dto)
        {
            var app = await _applicationService.UpdateStatusAsync(id, dto);
            if (app == null) return NotFound();
            return Ok(app);
        }
    }
}