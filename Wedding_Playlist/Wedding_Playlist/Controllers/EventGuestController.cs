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
        /// <returns>
        /// 200 OK<br/>
        /// [{EventGuestDTO},{EventGuestDTO},..]
        /// </returns>
        /// <example>
        /// GET: api/EventGuest
        /// </example>
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
        /// <param name="id">EventGuest ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// {EventGuestDTO}<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// GET: api/EventGuest/5
        /// </example>
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

        /// <summary>
        /// Returns all EventGuests for a specific Event ID
        /// </summary>
        /// <param name="id">Event ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// List of EventGuestDTO<br/>
        /// 404 Not Found if no matches
        /// </returns>
        /// <example>
        /// GET: api/EventGuest/event/3
        /// </example>
        [HttpGet("event/{id}")]
        public async Task<ActionResult<List<EventGuestDTO>>> GetEventGuestByEventID(int id)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.EventId == id).ToListAsync();
            if (eventGuests == null)
            {
                return NotFound();
            }
            var eventGuestDTO = eventGuests.Select(eventGuest => new EventGuestDTO
            {
                EventGuestId = eventGuest.EventGuestId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId
            }).ToList();

            return Ok(eventGuestDTO);
        }

        /// <summary>
        /// Returns all EventGuests for a specific Guest ID
        /// </summary>
        /// <param name="id">Guest ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// List of EventGuestDTO<br/>
        /// 404 Not Found if no matches
        /// </returns>
        /// <example>
        /// GET: api/EventGuest/guest/4
        /// </example>
        [HttpGet("guest/{id}")]
        public async Task<ActionResult<List<EventGuestDTO>>> GetEventGuestByGuestID(int id)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.GuestId == id).ToListAsync();
            if (eventGuests == null)
            {
                return NotFound();
            }
            var eventGuestDTO = eventGuests.Select(eventGuest => new EventGuestDTO
            {
                EventGuestId = eventGuest.EventGuestId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId
            }).ToList();

            return Ok(eventGuestDTO);
        }

        /// <summary>
        /// Creates a new EventGuest entry
        /// </summary>
        /// <param name="eventGuestDTO">New EventGuestDTO</param>
        /// <returns>
        /// 201 Created<br/>
        /// URI to newly created EventGuest<br/>
        /// 400 Bad Request if data is invalid
        /// </returns>
        /// <example>
        /// POST: api/EventGuest<br/>
        /// Body: { "eventId": 1, "guestId": 2 }
        /// </example>
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

        /// <summary>
        /// Updates an existing EventGuest entry
        /// </summary>
        /// <param name="id">EventGuest ID</param>
        /// <param name="eventGuestDTO">Updated EventGuestDTO</param>
        /// <returns>
        /// 200 OK<br/>
        /// Updated object<br/>
        /// 400 Bad Request if ID mismatch<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// PUT: api/EventGuest/5<br/>
        /// Body: { "eventGuestId": 5, "eventId": 1, "guestId": 2 }
        /// </example>
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

        /// <summary>
        /// Deletes an EventGuest entry by ID
        /// </summary>
        /// <param name="id">EventGuest ID</param>
        /// <returns>
        /// 200 OK<br/>
        /// Deleted object<br/>
        /// 404 Not Found if not found
        /// </returns>
        /// <example>
        /// DELETE: api/EventGuest/5
        /// </example>
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
