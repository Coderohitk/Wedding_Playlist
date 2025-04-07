using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Microsoft.EntityFrameworkCore;
namespace Wedding_Playlist.Services

{
    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _context;

        public SongService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Song>> GetAllSongs()
        {
            List<Song> songs = await _context.Songs.ToListAsync();
            List<Song> songList = new List<Song>();
            foreach (var song in songs)
            {
                ICollection<EventSong> eventSongs = new List<EventSong>();
                ICollection<PlaylistSong> playlistSongs = new List<PlaylistSong>();
                ICollection<GuestSongRequest> guestSongRequests = new List<GuestSongRequest>();
                songList.Add(new Song()
                {
                    SongId = song.SongId,
                    Title = song.Title,
                    Artist = song.Artist,
                    Genre = song.Genre,
                    Description = song.Description,
                    EventSongs = eventSongs,
                    PlaylistSongs = playlistSongs,
                    GuestSongRequests = guestSongRequests
                });
            }
            return songList;

        }
        public async Task<Song> GetSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return null;
            }
            Song song1 = new Song()
            {
                SongId = song.SongId,
                Title = song.Title,
                Artist = song.Artist,
                Genre = song.Genre,
                Description = song.Description,
                EventSongs = song.EventSongs,
                PlaylistSongs = song.PlaylistSongs,
                GuestSongRequests = song.GuestSongRequests
            };
            return song1;
        }
        public async Task<ServiceResponse> CreateSong(SongDTO songDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Song song = new Song()
            {
                Title = songDTO.Title,
                Artist = songDTO.Artist,
                Genre = songDTO.Genre,
                Description = songDTO.Description
            };
            try
            {
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(ex.Message);
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = song.SongId;
            return serviceResponse;

        }
        public async Task<ServiceResponse> UpdateSong(SongDTO songDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            if (songDTO.SongId == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("SongID cannot be null");
                return serviceResponse;
            }
            Song addsong = new Song()
            {
                SongId = songDTO.SongId,
                Title = songDTO.Title,
                Artist = songDTO.Artist,
                Genre = songDTO.Genre,
                Description = songDTO.Description
            };
            _context.Entry(addsong).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Song not found");
                return serviceResponse;

            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteSong(int id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Song not found");
                return serviceResponse;
            }
            try
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }

    }
}
