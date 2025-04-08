using Wedding_Playlist.Data;

using Wedding_Playlist.Models;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Interfaces;
namespace CoreEntityFramework.Services
{
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _context;
        public GuestService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Guest>> GetGuests()
        {
            List<Guest> guests = await _context.Guests.ToListAsync();
            List<Guest> guestlist = new List<Guest>();
            foreach (var guest in guests)
            {
                ICollection<EventGuest> eventGuests = await _context.EventGuests.Where(e => e.GuestId == guest.GuestId).ToListAsync();
                guestlist.Add(new Guest()
                {
                    GuestId = guest.GuestId,
                    Name = guest.Name,
                    Email = guest.Email,
                    RSVP_Status = guest.RSVP_Status,
                    Side = guest.Side,
                    EventGuests = eventGuests
                });
            }
            return guestlist;
        }
        public async Task<Guest> GetGuestById(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                return null;
            }
            Guest guests = new Guest()
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                RSVP_Status = guest.RSVP_Status,
                Side = guest.Side,
                EventGuests = guest.EventGuests
            };
            return guests;
        }
        public async Task<ServiceResponse> AddGuest(GuestDTO guestDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Guest guest = new Guest()
            {
                Name = guestDTO.Name,
                Email = guestDTO.Email,
                RSVP_Status = guestDTO.RSVP_Status,
                Side = guestDTO.Side
            };
            try
            {
                _context.Guests.Add(guest);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = guest.GuestId;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateGuest(GuestDTO guest)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
            if (guest.GuestId == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("No Guest Id Given");
                return serviceResponse;
            }
            Guest addguest = new Guest()
            {
                GuestId = guest.GuestId,
                Name = guest.Name,
                Email = guest.Email,
                RSVP_Status = guest.RSVP_Status,
                Side = guest.Side
            };
            _context.Entry(addguest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Guest Already Exists");
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteGuest(int id)
        {
            ServiceResponse response = new ServiceResponse();
            var guest = await _context.Guests.FindAsync(id);
            if (guest == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Guest Not Found");
                return response;
            }
            try
            {
                _context.Guests.Remove(guest);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add(e.Message);
                return response;
            }
            response.Status = ServiceResponse.ServiceStatus.Deleted;
            return response;
        }
    }
}
