using FestivalAppAPI.DTOs.Contact;
using FestivalAppAPI.Entities;
using FestivalAppAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FestivalAppAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Send(ContactMessageDto dto)
        {
            var message = new ContactMessage
            {
                Name = dto.Name,
                Email = dto.Email,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Message sent" });
        }

        [Authorize(Roles = "admin")]
        [HttpGet("messages")]
public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
            return Ok(messages);
        }
    }
}