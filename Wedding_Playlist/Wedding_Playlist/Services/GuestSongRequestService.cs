using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Microsoft.EntityFrameworkCore;
namespace CoreEntityFramework.Services
{
    public class GuestSongRequestService : IGuestSongRequestService
    {
        private readonly ApplicationDbContext _context;
        public GuestSongRequestService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GuestSongRequest>> GetAllGuestSongRequests()
        {
            List<GuestSongRequest> guestSongRequests = await _context.GuestSongRequests.ToListAsync();
            List<GuestSongRequest> guestSongRequestList = new List<GuestSongRequest>();
            foreach (var guestSongRequest in guestSongRequests)
            {
                guestSongRequestList.Add(new GuestSongRequest()
                {
                    RequestID = guestSongRequest.RequestID,
                    GuestID = guestSongRequest.GuestID,
                    SongID = guestSongRequest.SongID,

                    EventID = guestSongRequest.EventID,
                    Status = guestSongRequest.Status,
                    Event = guestSongRequest.Event,
                    Song = guestSongRequest.Song,
                    Guest = guestSongRequest.Guest
                });
            }
            return guestSongRequestList;
        }
        public async Task<GuestSongRequest> GetGuestSongRequest(int id)
        {
            var guestSongRequest = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequest == null)
            {
                return null;
            }
            GuestSongRequest guestSongRequestDTO = new GuestSongRequest()
            {
                RequestID = guestSongRequest.RequestID,
                GuestID = guestSongRequest.GuestID,
                SongID = guestSongRequest.SongID
            };
            return guestSongRequestDTO;
        }
        public async Task<ServiceResponse> CreateGuestSongRequest(GuestSongRequestDTO guestSongRequestDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            GuestSongRequest guestSongRequest = new GuestSongRequest()
            {
                GuestID = guestSongRequestDTO.GuestID,
                SongID = guestSongRequestDTO.SongID,
                EventID = guestSongRequestDTO.EventID,
                Status = guestSongRequestDTO.Status
            };
            try
            {
                _context.GuestSongRequests.Add(guestSongRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(ex.Message);
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = guestSongRequest.RequestID;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateGuestSongRequest(GuestSongRequestDTO guestSongRequestDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            if (guestSongRequestDTO.RequestID == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("RequestID cannot be null");
                return serviceResponse;
            }
            GuestSongRequest addGuestSongRequest = new GuestSongRequest()
            {
                RequestID = guestSongRequestDTO.RequestID,
                GuestID = guestSongRequestDTO.GuestID,
                SongID = guestSongRequestDTO.SongID,
                EventID = guestSongRequestDTO.EventID,
                Status = guestSongRequestDTO.Status
            };
            _context.Entry(addGuestSongRequest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("RequestID not found");
                return serviceResponse;

            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            return serviceResponse;
        }
        public async Task<ServiceResponse> DeleteGuestSongRequest(int id)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            var guestSongRequest = await _context.GuestSongRequests.FindAsync(id);
            if (guestSongRequest == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("RequestID not found");
                return serviceResponse;
            }
            try
            {
                _context.GuestSongRequests.Remove(guestSongRequest);
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