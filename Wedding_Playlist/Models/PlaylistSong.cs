using System.ComponentModel.DataAnnotations;

namespace Wedding_Playlist.Models
{
    public class PlaylistSong
    {
        [Key]
        public int PlaylistSongId { get; set; }
        public int Order { get; set; }

        public int PlaylistID { get; set; }


        public int SongID { get; set; }

        public Playlist Playlist { get; set; }
        public Song Song { get; set; }
    }
    public class PlaylistSongDTO
    {
        public int PlaylistSongId { get; set; }
        public int Order { get; set; }
        public int PlaylistID { get; set; }
        public int SongID { get; set; }
    }
}
