using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Wedding_Playlist.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreEntityFramework.Services
{
    public class EventGuestService : IEventGuestService
    {
        private readonly ApplicationDbContext _context;

        public EventGuestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventGuest>> GetEventGuests()
        {
            List<EventGuest> eventGuests = await _context.EventGuests.ToListAsync();
            List<EventGuest> eventGuestsList = new List<EventGuest>();
            foreach (EventGuest eventGuest in eventGuests)
            {
                eventGuestsList.Add(new EventGuest()
                {
                    EventGuestId = eventGuest.EventGuestId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId,
                    Guest = eventGuest.Guest,
                });
            }
            return eventGuestsList;
        }

        public async Task<EventGuest> GetEventGuestById(int id)
        {
            var eventGuest = await _context.EventGuests.FindAsync(id);
            if (eventGuest == null)
                return null;

            return new EventGuest
            {
                EventGuestId = eventGuest.EventGuestId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId
            };
        }

        public async Task<List<EventGuest>> GetEventGuestsbyEventId(int id)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.EventId == id).ToListAsync();
            if (eventGuests == null)
                return null;

            return eventGuests.Select(eg => new EventGuest
            {
                EventGuestId = eg.EventGuestId,
                EventId = eg.EventId,
                GuestId = eg.GuestId
            }).ToList();
        }

        public async Task<List<EventGuest>> GetEventGuestsbyGuestId(int id)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.GuestId == id).ToListAsync();
            if (eventGuests == null)
                return null;

            return eventGuests.Select(eg => new EventGuest
            {
                EventGuestId = eg.EventGuestId,
                EventId = eg.EventId,
                GuestId = eg.GuestId
            }).ToList();
        }

        public async Task<ServiceResponse> AddEventGuest(EventGuestDTO eventGuestDTO)
        {
            var response = new ServiceResponse();
            var eventGuest = new EventGuest
            {
                EventId = eventGuestDTO.EventId,
                GuestId = eventGuestDTO.GuestId
            };

            try
            {
                _context.EventGuests.Add(eventGuest);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Created;
                response.Messages.Add("Event Guest Added Successfully");
            }
            catch (Exception e)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add(e.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> UpdateEventGuest(EventGuestDTO eventGuestDTO)
        {
            var response = new ServiceResponse();
            var eventGuest = new EventGuest
            {
                EventGuestId = eventGuestDTO.EventGuestId,
                EventId = eventGuestDTO.EventId,
                GuestId = eventGuestDTO.GuestId
            };

            try
            {
                _context.EventGuests.Update(eventGuest);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
                response.Messages.Add("Event Guest Updated Successfully");
            }
            catch (Exception e)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add(e.Message);
            }

            return response;
        }

        public async Task<ServiceResponse> DeleteEventGuest(int id)
        {
            var response = new ServiceResponse();
            var eventGuest = await _context.EventGuests.FindAsync(id);

            if (eventGuest == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Event Guest Not Found");
                return response;
            }

            try
            {
                _context.EventGuests.Remove(eventGuest);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
                response.Messages.Add("Event Guest Deleted Successfully");
            }
            catch (Exception e)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add(e.Message);
            }

            return response;
        }
    }
}
