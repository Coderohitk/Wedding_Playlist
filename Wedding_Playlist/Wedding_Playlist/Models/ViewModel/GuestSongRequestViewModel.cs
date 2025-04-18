namespace Wedding_Playlist.Models.ViewModel
{
    public class GuestSongRequestViewModel
    {
        public int RequestID { get; set; }
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public int SongID { get; set; }
        public string SongTitle { get; set; }
        public string SongArtist { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string Status { get; set; }
    }
}
