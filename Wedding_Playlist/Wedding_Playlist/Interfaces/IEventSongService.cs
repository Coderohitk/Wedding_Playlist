using Wedding_Playlist.Models;
namespace Wedding_Playlist.Interfaces
{
    public interface IEventSongService
    {
        Task<IEnumerable<EventSong>> GetEventSongs();
        Task<EventSong> GetEventSongById(int id);
        Task<List<EventSong>> GetEventSongbyEventId(int id);
        Task<ServiceResponse> AddEventSong(EventSongDTO eventSongDTO);
        Task<ServiceResponse> UpdateEventSong(EventSongDTO eventSongDTO);
        Task<ServiceResponse> DeleteEventSong(int id);
    }
}