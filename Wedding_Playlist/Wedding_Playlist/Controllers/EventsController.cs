using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDTO>>> GetEvents()
        {
            var events = await _context.Events
                .Select(e => new EventDTO
                {
                    EventId = e.EventId,
                    Name = e.Name,
                    Date = e.Date,
                    Location = e.Location
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDTO>> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null) return NotFound();

            var eventDTO = new EventDTO
            {
                EventId = eventItem.EventId,
                Name = eventItem.Name,
                Date = eventItem.Date,
                Location = eventItem.Location
            };

            return Ok(eventDTO);
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent([FromBody] EventDTO eventDTO)
        {
            var eventItem = new Event
            {
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            };

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.EventId }, eventDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDTO eventDTO)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null) return NotFound();

            eventItem.Name = eventDTO.Name;
            eventItem.Date = eventDTO.Date;
            eventItem.Location = eventDTO.Location;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
