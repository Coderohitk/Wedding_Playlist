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

        // GET: api/EventGuest
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

        // GET: api/EventGuest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventGuestDTO>> GetEventGuest(int id)
        {
            var eventGuest = await _context.EventGuests
                .Where(s => s.EventGuestId == id).FirstOrDefaultAsync();
            if (eventGuest == null)
            {
                return NotFound();
            }
            var eventGuestDTO = new EventGuestDTO
            {
                EventGuestId = eventGuest.EventGuestId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId
            };
            return Ok(eventGuestDTO);
        }
        public async Task<ActionResult<List<EventGuestDTO>>> GetEventGuestByEventID(int id)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.EventId == id).ToListAsync();
            if (eventGuests == null)
            {
                return NotFound();
            }
            var eventGuestDTO = new List<EventGuestDTO>();
            foreach (var eventGuest in eventGuests)
            {
                var eventguestlist = new EventGuestDTO
                {
                    EventGuestId = eventGuest.EventGuestId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId
                };
                eventGuestDTO.Add(eventguestlist);
            }
            return Ok(eventGuestDTO);
        }
        [HttpPost]
        public async Task<ActionResult<EventGuestDTO>> CreateEventGuest([FromBody] EventGuestDTO eventGuestDTO)
        {
            if (eventGuestDTO == null)
            {
                return BadRequest();
            }
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
        [HttpPut("{id}")]
        public async Task<ActionResult<EventGuestDTO>> UpdateEventGuest([FromRoute] int id, [FromBody] EventGuestDTO eventGuestDTO)
        {
            if (id != eventGuestDTO.EventGuestId)
            {
                return BadRequest();
            }
            var eventGuestToUpdate = await _context.EventGuests.FindAsync(id);
            if (eventGuestToUpdate == null)
            {
                return NotFound();
            }

            eventGuestToUpdate.EventId = eventGuestDTO.EventId;
            eventGuestToUpdate.GuestId = eventGuestDTO.GuestId;

            await _context.SaveChangesAsync();

            return Ok(eventGuestToUpdate);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventGuestDTO>> DeleteEventGuest([FromRoute] int id)
        {
            var eventGuest = await _context.EventGuests.FindAsync(id);
            if (eventGuest == null)
            {
                return NotFound();
            }

            _context.EventGuests.Remove(eventGuest);
            await _context.SaveChangesAsync();

            return Ok(eventGuest);
        }
    }
}