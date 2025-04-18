using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventGuestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventGuestController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of all EventGuest entries
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventGuestDTO>>> GetAllEventGuests()
        {
            var eventGuests = await _context.EventGuests
                .Select(s => new EventGuestDTO
                {
                    EventGuestId = s.EventGuestId,
                    EventId = s.EventId,
                    GuestId = s.GuestId
                }).ToListAsync();

            return Ok(eventGuests);
        }

        /// <summary>
        /// Returns a specific EventGuest by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EventGuestDTO>> GetEventGuest(int id)
        {
            var eventGuest = await _context.EventGuests.FindAsync(id);
            if (eventGuest == null)
            {
                return NotFound();
            }

            var dto = new EventGuestDTO
            {
                EventGuestId = eventGuest.EventGuestId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId
            };

            return Ok(dto);
        }

        /// <summary>
        /// Returns all EventGuests for a specific Event ID
        /// </summary>
        [HttpGet("event/{id}")]
        public async Task<ActionResult<List<EventGuestDTO>>> GetEventGuestByEventID(int id)
        {
            var eventGuests = await _context.EventGuests
                .Where(x => x.EventId == id)
                .Select(x => new EventGuestDTO
                {
                    EventGuestId = x.EventGuestId,
                    EventId = x.EventId,
                    GuestId = x.GuestId
                }).ToListAsync();

            return eventGuests.Any() ? Ok(eventGuests) : NotFound();
        }

        /// <summary>
        /// Returns all EventGuests for a specific Guest ID
        /// </summary>
        [HttpGet("guest/{id}")]
        public async Task<ActionResult<List<EventGuestDTO>>> GetEventGuestByGuestID(int id)
        {
            var eventGuests = await _context.EventGuests
                .Where(x => x.GuestId == id)
                .Select(x => new EventGuestDTO
                {
                    EventGuestId = x.EventGuestId,
                    EventId = x.EventId,
                    GuestId = x.GuestId
                }).ToListAsync();

            return eventGuests.Any() ? Ok(eventGuests) : NotFound();
        }

        /// <summary>
        /// Creates a new EventGuest entry
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EventGuestDTO>> CreateEventGuest([FromBody] EventGuestDTO eventGuestDTO)
        {
            if (eventGuestDTO == null) return BadRequest();

            var newEventGuest = new EventGuest
            {
                EventId = eventGuestDTO.EventId,
                GuestId = eventGuestDTO.GuestId
            };

            _context.EventGuests.Add(newEventGuest);
            await _context.SaveChangesAsync();

            eventGuestDTO.EventGuestId = newEventGuest.EventGuestId;
            return CreatedAtAction(nameof(GetEventGuest), new { id = newEventGuest.EventGuestId }, eventGuestDTO);
        }

        /// <summary>
        /// Updates an existing EventGuest entry
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<EventGuestDTO>> UpdateEventGuest([FromRoute] int id, [FromBody] EventGuestDTO eventGuestDTO)
        {
            if (id != eventGuestDTO.EventGuestId) return BadRequest();

            var existing = await _context.EventGuests.FindAsync(id);
            if (existing == null) return NotFound();

            existing.EventId = eventGuestDTO.EventId;
            existing.GuestId = eventGuestDTO.GuestId;

            await _context.SaveChangesAsync();
            return Ok(eventGuestDTO);
        }

        /// <summary>
        /// Deletes an EventGuest entry by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventGuestDTO>> DeleteEventGuest([FromRoute] int id)
        {
            var existing = await _context.EventGuests.FindAsync(id);
            if (existing == null) return NotFound();

            _context.EventGuests.Remove(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }
    }
}
