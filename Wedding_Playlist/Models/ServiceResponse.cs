using Microsoft.AspNetCore.SignalR;

namespace Wedding_Playlist.Models
{
    public class ServiceResponse
    {
        public enum ServiceStatus { NotFound, Created, Updated, Deleted, Error, Success }
        public ServiceStatus Status { get; set; }
        public int CreatedId { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public bool Success { get; set; }

    }

}