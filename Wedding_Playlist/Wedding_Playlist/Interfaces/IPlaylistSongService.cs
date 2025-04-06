using Wedding_Playlist.Models;

namespace Wedding_Playlist.Interfaces
{
    public interface IPlaylistSongService
    {
        Task<IEnumerator<PlaylistSong>> GetAllPlaylistSongs();
        Task<PlaylistSong> GetPlaylistSong(int id);
        Task<IEnumerator<PlaylistSong>> GetPlaylistSongsByPlaylist(int playlistId);
        Task<ServiceResponse> CreatePlaylistSong(PlaylistSongDTO playlistSongDTO);
        Task<ServiceResponse> UpdatePlaylistSong(PlaylistSongDTO playlistSongDTO);
        Task<ServiceResponse> DeletePlaylistSong(int id);
    }
}