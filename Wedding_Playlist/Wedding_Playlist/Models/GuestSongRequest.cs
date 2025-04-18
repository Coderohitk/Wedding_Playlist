using System.ComponentModel.DataAnnotations;

namespace Wedding_Playlist.Models
{
    public class GuestSongRequest
    {
        [Key]
        public int RequestID { get; set; }

        public int EventID { get; set; }
        public int GuestID { get; set; }
        public int SongID { get; set; }

        public string Status { get; set; } = "Pending";

        // Navigation Properties
        public Event Event { get; set; }
        public Guest Guest { get; set; }
        public Song Song { get; set; }
    }
    public class GuestSongRequestDTO
    {
        public int RequestID { get; set; }
        public int EventID { get; set; }
        public int GuestID { get; set; }
        public int SongID { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
