using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
namespace Wedding_Playlist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GuestSongRequestAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GuestSongRequestAPIController(ApplicationDbContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestSongRequest>>> GetGuestSongRequests()
        {
            var guestSongRequests = await _context.GuestSongRequests
                .Include(gsr => gsr.Event)  // Include related Event
                .Include(gsr => gsr.Song)   // Include related Song
                .Include(gsr => gsr.Guest)  // Include related Guest
                .Select(s => new
                {
                    s.RequestID,
                    s.GuestID,
                    GuestName = s.Guest.Name,  // Guest Name
                    s.SongID,
                    SongTitle = s.Song.Title,  // Song Title
                    s.EventID,
                    EventName = s.Event.Name,  // Event Name
                    s.Status
                })
                .ToListAsync();

            return Ok(guestSongRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestSongRequest>> GetGuestSongRequest(int id)
        {
            var guestSongRequest = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequest == null)
            {
                return NotFound();
            }
            var guestSongRequestnew = new GuestSongRequest()
            {
                RequestID = guestSongRequest.RequestID,
                EventID = guestSongRequest.EventID,
                GuestID = guestSongRequest.GuestID,
                SongID = guestSongRequest.SongID,
                Status = guestSongRequest.Status,
            };
            return Ok(guestSongRequest);
        }
        [HttpPost]
        public async Task<ActionResult<GuestSongRequest>> CreateGuestSongRequest(GuestSongRequest guestSongRequest)
        {
            var newGuestSongRequest = new GuestSongRequest()
            {
                EventID = guestSongRequest.EventID,
                GuestID = guestSongRequest.GuestID,
                SongID = guestSongRequest.SongID,
                Status = guestSongRequest.Status,
            };
            _context.GuestSongRequests.Add(newGuestSongRequest);
            await _context.SaveChangesAsync();
            return Ok(newGuestSongRequest);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<GuestSongRequest>> UpdateGuestSongRequest(int id, GuestSongRequest guestSongRequest)
        {
            var guestSongRequestToUpdate = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequestToUpdate == null)
            {
                return NotFound();
            }
            guestSongRequestToUpdate.EventID = guestSongRequest.EventID;
            guestSongRequestToUpdate.GuestID = guestSongRequest.GuestID;
            guestSongRequestToUpdate.SongID = guestSongRequest.SongID;
            guestSongRequestToUpdate.Status = guestSongRequest.Status;
            await _context.SaveChangesAsync();
            return Ok(guestSongRequestToUpdate);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<GuestSongRequest>> DeleteGuestSongRequest(int id)
        {
            var guestSongRequestToDelete = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequestToDelete == null)
            {
                return NotFound();
            }
            _context.GuestSongRequests.Remove(guestSongRequestToDelete);
            await _context.SaveChangesAsync();
            return Ok(guestSongRequestToDelete);
        }
    }
}