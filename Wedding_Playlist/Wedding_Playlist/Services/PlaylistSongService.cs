using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Wedding_Playlist.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace CoreEntityFramework.Services
{
    public class PlaylistSongService : IPlaylistSongService
    {
        private readonly ApplicationDbContext _context;
        public PlaylistSongService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PlaylistSong>> GetAllPlaylistSongs()
        {
            List<PlaylistSong> playlistSongs = await _context.PlaylistSongs.ToListAsync();
            List<PlaylistSong> playlistSongList = new List<PlaylistSong>();
            foreach (var playlistSong in playlistSongs)
            {
                playlistSongList.Add(new PlaylistSong()
                {
                    PlaylistSongId = playlistSong.PlaylistSongId,
                    PlaylistID = playlistSong.PlaylistID,
                    SongID = playlistSong.SongID,
                    Song = playlistSong.Song,
                    Playlist = playlistSong.Playlist
                });
            }
            return playlistSongList;
        }
        public async Task<PlaylistSong> GetPlaylistSong(int id)
        {
            var playlistSong = await _context.PlaylistSongs.FindAsync(id);
            if (playlistSong == null)
            {
                return null;
            }
            PlaylistSong playlistSongDTO = new PlaylistSong()
            {
                PlaylistSongId = playlistSong.PlaylistSongId,
                PlaylistID = playlistSong.PlaylistID,
                SongID = playlistSong.SongID
            };
            return playlistSongDTO;
        }
        public async Task<ServiceResponse> CreatePlaylistSong(PlaylistSongDTO playlistSongDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            PlaylistSong playlistSong = new PlaylistSong()
            {
                PlaylistID = playlistSongDTO.PlaylistID,
                SongID = playlistSongDTO.SongID
            };
            try
            {
                _context.PlaylistSongs.Add(playlistSong);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(ex.Message);
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = playlistSong.PlaylistSongId;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdatePlaylistSong(PlaylistSongDTO playlistSongDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            if (playlistSongDTO.PlaylistSongId == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("PlaylistSongID cannot be null");
                return serviceResponse;
            }
            PlaylistSong addPlaylistSong = new PlaylistSong()
            {
                PlaylistSongId = playlistSongDTO.PlaylistSongId,
                PlaylistID = playlistSongDTO.PlaylistID,
                SongID = playlistSongDTO.SongID
            };
            _context.Entry(addPlaylistSong).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("PlaylistSong not found");
                return serviceResponse;

            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeletePlaylistSong(int id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var playlistSong = await _context.PlaylistSongs.FindAsync(id);
            if (playlistSong == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("PlaylistSong not found");
                return serviceResponse;
            }
            try
            {
                _context.PlaylistSongs.Remove(playlistSong);
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
