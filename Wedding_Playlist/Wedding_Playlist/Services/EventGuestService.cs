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
            {
                return null;
            }
            EventGuest eventGuestDTO = new EventGuest()
            {
                EventGuestId = eventGuest.EventGuestId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId


            };
            return eventGuestDTO;
        }
        public async Task<List<EventGuest>> GetEventGuestsbyEventId(int id)
        {
            var eventguest = await _context.EventGuests.Where(x => x.EventId == id).ToListAsync();
            if (eventguest == null)
            {
                return null;
            }
            List<EventGuest> eventGuestList = new List<EventGuest>();
            foreach (var eventGuest in eventguest)
            {
                eventGuestList.Add(new EventGuest()
                {
                    EventGuestId = eventGuest.EventGuestId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId,


                });
            }
            return eventGuestList;
        }
        public async Task<List<EventGuest>> GetEventGuestsbyGuestId(int id)
        {
            var eventguest = await _context.EventGuests.Where(x => x.GuestId == id).ToListAsync();
            if (eventguest == null)
            {
                return null;
            }
            List<EventGuest> eventGuestList = new List<EventGuest>();
            foreach (var eventGuest in eventguest)
            {
                eventGuestList.Add(new EventGuest()
                {
                    EventGuestId = eventGuest.EventGuestId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId,


                });
            }
            return eventGuestList;
        }

        public async Task<ServiceResponse> AddEventGuest(EventGuestDTO eventGuestDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventGuest eventGuest = new EventGuest()
            {
                EventId = eventGuestDTO.EventId,
                GuestId = eventGuestDTO.GuestId
            };
            try
            {
                _context.EventGuests.Add(eventGuest);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.Messages.Add("Event Guest Added Successfully");
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateEventGuest(EventGuestDTO eventGuestDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventGuest eventGuest = new EventGuest()
            {
                EventGuestId = eventGuestDTO.EventGuestId,
                EventId = eventGuestDTO.EventId,
                GuestId = eventGuestDTO.GuestId
            };
            try
            {
                _context.EventGuests.Update(eventGuest);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add("Event Guest Updated Successfully");
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteEventGuest(int id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventGuest eventGuest = await _context.EventGuests.FindAsync(id);
            if (eventGuest == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Event Guest Not Found");
                return serviceResponse;
            }
            _context.EventGuests.Remove(eventGuest);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            serviceResponse.Messages.Add("Event Guest Deleted Successfully");
            return serviceResponse;
        }

    }
}