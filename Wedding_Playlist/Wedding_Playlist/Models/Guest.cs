using System.ComponentModel.DataAnnotations;

namespace Wedding_Playlist.Models
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string RSVP_Status { get; set; }
        public string Side { get; set; }
        public ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();
        public ICollection<GuestSongRequest> GuestSongRequests { get; set; } = new List<GuestSongRequest>();
    }
}
