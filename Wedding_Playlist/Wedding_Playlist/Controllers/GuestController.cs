using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Data;

namespace WeddingPlaylist.Controllers
{
    [Route("api/[controller]")] // Base route: api/guest
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/guest - Retrieve all guests
        [HttpGet]
        [Route(template:"ListGuest")]
        public async Task<ActionResult<IEnumerable<GuestDTO>>> GetGuests()
        {
            var guests = await _context.Guests
                .Select(g => new GuestDTO
                {
                    GuestId = g.GuestId,
                    Name = g.Name,
                    Email = g.Email,
                    RSVP_Status = g.RSVP_Status,
                    Side = g.Side
                })
                .ToListAsync();

            return Ok(guests);
        }

        // ✅ GET: api/guest/{id} - Retrieve a guest by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GuestDTO>> GetGuest([FromRoute] int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            var guestDTO = new GuestDTO
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                RSVP_Status = guest.RSVP_Status,
                Side = guest.Side
            };

            return Ok(guestDTO);
        }

        // ✅ POST: api/guest - Create a new guest
        [HttpPost]
        public async Task<ActionResult<GuestDTO>> CreateGuest([FromBody] GuestDTO guestDTO)
        {
            if (guestDTO == null)
            {
                return BadRequest("Invalid guest data.");
            }

            var guest = new Guest
            {
                Name = guestDTO.Name,
                Email = guestDTO.Email,
                RSVP_Status = guestDTO.RSVP_Status,
                Side = guestDTO.Side
            };

            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();

            guestDTO.GuestId = guest.GuestId; // Assign the newly created ID
            return CreatedAtAction(nameof(GetGuest), new { id = guest.GuestId }, guestDTO);
        }

        // ✅ PUT: api/guest/{id} - Update guest details
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGuest([FromRoute] int id, [FromBody] GuestDTO guestDTO)
        {
            if (id != guestDTO.GuestId)
            {
                return BadRequest("Guest ID mismatch.");
            }

            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            // Update the guest details
            guest.Name = guestDTO.Name;
            guest.Email = guestDTO.Email;
            guest.RSVP_Status = guestDTO.RSVP_Status;
            guest.Side = guestDTO.Side;

            _context.Entry(guest).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: api/guest/{id} - Delete a guest
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest([FromRoute] int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
