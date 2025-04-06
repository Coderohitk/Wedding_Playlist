using Wedding_Playlist.Data;
using System;

using Wedding_Playlist.Models;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Interfaces;

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
            List<EventGuest> eventGuestList = new List<EventGuest>();
            foreach (var eventGuest in eventGuests)
            {
                eventGuestList.Add(new EventGuest()
                {
                    GuestEventId = eventGuest.GuestEventId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId,
                    EventMen = eventGuest.EventMen,
                    EventWomen = eventGuest.EventWomen,
                    EventKids = eventGuest.EventKids
                });
            }
            return eventGuestList;
        }
        public async Task<EventGuest> GetEventGuestById(int id)
        {
            var eventGuest = await _context.EventGuests.FindAsync(id);
            if (eventGuest == null)
            {
                return null;
            }
            EventGuest eventGuests = new EventGuest()
            {
                GuestEventId = eventGuest.GuestEventId,
                EventId = eventGuest.EventId,
                EventMen = eventGuest.EventMen,
                EventWomen = eventGuest.EventWomen,
                EventKids = eventGuest.EventKids
            };
            return eventGuests;
        }
        public async Task<List<EventGuest>> GetEventGuestByEvent(int eventId)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.EventId == eventId).ToListAsync();
            if (eventGuests == null)
            {
                return null;
            }
            List<EventGuest> eventGuestList = new List<EventGuest>();
            foreach (var eventGuest in eventGuests)
            {
                eventGuestList.Add(new EventGuest()
                {
                    GuestEventId = eventGuest.GuestEventId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId,
                    EventMen = eventGuest.EventMen,
                    EventWomen = eventGuest.EventWomen,
                    EventKids = eventGuest.EventKids
                });

            }
            return eventGuestList;
        }
        public async Task<List<EventGuest>> GetEventGuestByGuest(int guestId)
        {
            var eventGuests = await _context.EventGuests.Where(x => x.GuestId == guestId).ToListAsync();
            if (eventGuests == null)
            {
                return null;
            }
            List<EventGuest> eventGuestList = new List<EventGuest>();
            foreach (var eventGuest in eventGuests)
            {
                eventGuestList.Add(new EventGuest()
                {
                    GuestEventId = eventGuest.GuestEventId,
                    EventId = eventGuest.EventId,
                    GuestId = eventGuest.GuestId,
                    EventMen = eventGuest.EventMen,
                    EventWomen = eventGuest.EventWomen,
                    EventKids = eventGuest.EventKids
                });

            }
            return eventGuestList;
        }
        public async Task<List<EventGuest>> GetEventGuestsByIsRSVPAccepted(bool isRSVPAccepted)
        {
            return await _context.EventGuests
                                 .Where(e => e.IsRSVPAccepted == isRSVPAccepted)
                                 .ToListAsync();
        }
        public async Task<List<EventGuest>> GetEventGuestsByEventMen(int eventMen)
        {
            return await _context.EventGuests
                                 .Where(e => e.EventMen == eventMen)
                                 .ToListAsync();
        }
        public async Task<List<EventGuest>> GetEventGuestsByEventWomen(int eventWomen)
        {
            return await _context.EventGuests
                                 .Where(e => e.EventWomen == eventWomen)
                                 .ToListAsync();
        }
        public async Task<List<EventGuest>> GetEventGuestsByEventKids(int eventKids)
        {
            return await _context.EventGuests
                                 .Where(e => e.EventKids == eventKids)
                                 .ToListAsync();
        }
        public async Task<ServiceResponse> AddEventGuest(EventGuestDto eventGuestDto)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventGuest eventGuest = new EventGuest()
            {

                EventId = eventGuestDto.EventId,
                EventMen = eventGuestDto.EventMen,
                EventWomen = eventGuestDto.EventWomen,
                EventKids = eventGuestDto.EventKids,
                GuestId = eventGuestDto.GuestId,
                IsRSVPAccepted = eventGuestDto.IsRSVPAccepted
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
            serviceResponse.CreatedId = eventGuest.GuestEventId;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateEventGuest(EventGuestDto eventGuest)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventGuest addEventGuest = new EventGuest()
            {
                GuestEventId = eventGuest.GuestEventId,
                EventId = eventGuest.EventId,
                GuestId = eventGuest.GuestId,
                EventMen = eventGuest.EventMen,
                EventWomen = eventGuest.EventWomen,
                EventKids = eventGuest.EventKids
            };
            _context.Entry(addEventGuest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Update Failed");
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;

        }
        public async Task<ServiceResponse> DeleteEventGuest(int id)
        {
            ServiceResponse response = new ServiceResponse();
            var eventGuest = await _context.EventGuests.FindAsync(id);
            if (eventGuest == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("EventGuest Not Found");
                return response;
            }
            try
            {
                _context.EventGuests.Remove(eventGuest);
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
