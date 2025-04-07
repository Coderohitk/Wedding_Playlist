using Wedding_Playlist.Data;

using Wedding_Playlist.Models;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Interfaces;
namespace Wedding_Playlist.Services

{
    public class PlaylistService : IPlaylistService
    {
        private readonly ApplicationDbContext _context;
        public PlaylistService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Playlist>> GetAllPlaylists()
        {
            List<Playlist> playlists = await _context.Playlists.ToListAsync();
            List<Playlist> playlistList = new List<Playlist>();
            foreach (var playlist in playlists)
            {
                ICollection<PlaylistSong> playlistSongs = new List<PlaylistSong>();
                playlistList.Add(new Playlist()
                {
                    PlaylistID = playlist.PlaylistID,
                    Name = playlist.Name,
                    CreatedBy = playlist.CreatedBy,
                    PlaylistSongs = playlistSongs
                });
            }
            return playlistList;
        }
        public async Task<Playlist> GetPlaylist(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return null;
            }
            Playlist playlist1 = new Playlist()
            {
                PlaylistID = playlist.PlaylistID,
                Name = playlist.Name,
                CreatedBy = playlist.CreatedBy,
                PlaylistSongs = playlist.PlaylistSongs
            };
            return playlist1;
        }
        public async Task<ServiceResponse> CreatePlaylist(PlaylistDTO playlistDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            Playlist playlist = new Playlist()
            {
                Name = playlistDTO.Name,
                CreatedBy = playlistDTO.CreatedBy,

            };
            try
            {
                _context.Playlists.Add(playlist);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(ex.Message);
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = playlist.PlaylistID;
            return serviceResponse;

        }
        public async Task<ServiceResponse> UpdatePlaylist(PlaylistDTO playlistDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            if (playlistDTO.PlaylistID == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("PlaylistID cannot be null");
                return serviceResponse;
            }
            Playlist addsong = new Playlist()
            {
                PlaylistID = playlistDTO.PlaylistID,
                Name = playlistDTO.Name,
                CreatedBy = playlistDTO.CreatedBy
            };
            _context.Entry(addsong).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Playlist not found");
                return serviceResponse;

            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeletePlaylist(int id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Playlist not found");
                return serviceResponse;
            }
            try
            {
                _context.Playlists.Remove(playlist);
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