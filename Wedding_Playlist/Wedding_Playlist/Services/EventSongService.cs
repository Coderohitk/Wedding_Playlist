using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Wedding_Playlist.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CoreEntityFramework.Services
{
    public class EventSongService : IEventSongService
    {
        private readonly ApplicationDbContext _context;
        public EventSongService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EventSong>> GetEventSongs()
        {
            List<EventSong> eventSongs = await _context.EventSongs.ToListAsync();
            List<EventSong> eventSongsList = new List<EventSong>();
            foreach (EventSong eventSong in eventSongs)
            {
                eventSongsList.Add(new EventSong()
                {
                    EventSongId = eventSong.EventSongId,
                    EventId = eventSong.EventId,
                    SongId = eventSong.SongId,
                    Song = eventSong.Song,
                    Event=eventSong.Event



                });
            }
            return eventSongsList;
        }
        public async Task<EventSong> GetEventSongById(int id)
        {
            var eventSong = await _context.EventSongs.FindAsync(id);
            if (eventSong == null)
            {
                return null;
            }
            EventSong eventSongDTO = new EventSong()
            {
                EventSongId = eventSong.EventSongId,
                EventId = eventSong.EventId,
                SongId = eventSong.SongId


            };
            return eventSongDTO;
        }
        public async Task<List<EventSong>> GetEventSongbyEventId(int id)
        {
            var eventSong = await _context.EventSongs.Where(x => x.EventId == id).ToListAsync();
            if (eventSong == null)
            {
                return null;
            }
            List<EventSong> eventSongList = new List<EventSong>();
            foreach (var eventSongs in eventSong)
            {
                eventSongList.Add(new EventSong()
                {
                    EventSongId = eventSongs.EventSongId,
                    EventId = eventSongs.EventId,
                    SongId = eventSongs.SongId,


                });
            }
            return eventSongList;
        }

        public async Task<ServiceResponse> AddEventSong(EventSongDTO eventSongDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventSong eventSong = new EventSong()
            {
                EventId = eventSongDTO.EventId,
                SongId = eventSongDTO.SongId
            };
            try
            {
                _context.EventSongs.Add(eventSong);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.Messages.Add("Event Song Added Successfully");
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateEventSong(EventSongDTO eventSongDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventSong eventSong = new EventSong()
            {
                EventSongId = eventSongDTO.EventSongId,
                EventId = eventSongDTO.EventId,
                SongId = eventSongDTO.SongId
            };
            try
            {
                _context.EventSongs.Update(eventSong);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(e.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            serviceResponse.Messages.Add("Event Song Updated Successfully");
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteEventSong(int id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            EventSong eventSong = await _context.EventSongs.FindAsync(id);
            if (eventSong == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Event Song Not Found");
                return serviceResponse;
            }
            _context.EventSongs.Remove(eventSong);
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
            serviceResponse.Messages.Add("Event Song Deleted Successfully");
            return serviceResponse;
        }

    }
}