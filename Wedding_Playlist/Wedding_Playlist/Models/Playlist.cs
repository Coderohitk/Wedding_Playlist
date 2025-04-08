using System.ComponentModel.DataAnnotations;
namespace Wedding_Playlist.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistID { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; } = new List<PlaylistSong>();
    }
    public class PlaylistDTO
    {
        public int PlaylistID { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }
    public class PlaylistDetailsViewModel
    {
        public PlaylistDTO Playlist { get; set; }
        public List<SongDTO> Songs { get; set; } = new List<SongDTO>();
    }
}
