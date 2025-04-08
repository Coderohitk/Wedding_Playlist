using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;
using Wedding_Playlist.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace CoreEntityFramework.Services
{
    public class GuestSongRequestService : IGuestSongRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventSongService _eventSongService;
        private readonly ILogger<GuestSongRequestService> _logger;

        public GuestSongRequestService(ApplicationDbContext context, IEventSongService eventSongService, ILogger<GuestSongRequestService> logger)
        {
            _context = context;
            _eventSongService = eventSongService;
            _logger = logger;
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

            // Return a detached copy with all properties
            GuestSongRequest guestSongRequestDTO = new GuestSongRequest()
            {
                RequestID = guestSongRequest.RequestID,
                GuestID = guestSongRequest.GuestID,
                SongID = guestSongRequest.SongID,
                EventID = guestSongRequest.EventID,
                Status = guestSongRequest.Status
            };
            return guestSongRequestDTO;
        }
        public async Task<ServiceResponse> GetGuestSongRequestById(int id)
        {
            try
            {
                var guestSongRequest = await _context.GuestSongRequests.FirstOrDefaultAsync(g => g.RequestID == id);

                if (guestSongRequest == null)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Messages = new List<string> { $"Song request with ID {id} not found" }
                    };
                }

                return new ServiceResponse
                {
                    Success = true,
                    Status = ServiceResponse.ServiceStatus.Success,
                    Messages = new List<string> { "Song request retrieved successfully" }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving song request with ID {id}");
                return new ServiceResponse
                {
                    Success = false,
                    Status = ServiceResponse.ServiceStatus.Error,
                    Messages = new List<string> { $"Error retrieving song request: {ex.Message}" }
                };
            }
        }
        public async Task<List<GuestSongRequest>> GetGuestSongRequestByGuestId(int id)
        {
            var guestsong = await _context.GuestSongRequests.Where(x => x.GuestID == id).ToListAsync();
            if (guestsong == null)
            {
                return null;
            }
            List<GuestSongRequest> eventGuestList = new List<GuestSongRequest>();
            foreach (var eventGuest in guestsong)
            {
                eventGuestList.Add(new GuestSongRequest()
                {
                    RequestID = eventGuest.RequestID,
                    EventID = eventGuest.EventID,
                    GuestID = eventGuest.GuestID,
                    SongID = eventGuest.SongID,
                    Status = eventGuest.Status
                });
            }
            return eventGuestList;
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

                // If the song request is approved, add to EventSong table
                if (guestSongRequestDTO.Status == "Approved")
                {
                    // Check if this song is already in the event
                    var existingEventSong = await _context.EventSongs
                        .FirstOrDefaultAsync(es => es.EventId == guestSongRequestDTO.EventID &&
                                                 es.SongId == guestSongRequestDTO.SongID);

                    if (existingEventSong == null)
                    {
                        // Add the song to the event
                        var eventSongDTO = new EventSongDTO
                        {
                            EventId = guestSongRequestDTO.EventID,
                            SongId = guestSongRequestDTO.SongID
                        };

                        var eventSongResponse = await _eventSongService.AddEventSong(eventSongDTO);
                        if (eventSongResponse.Status != ServiceResponse.ServiceStatus.Created)
                        {
                            // Log error but don't fail the request
                            serviceResponse.Messages.Add("Song request was created as approved but could not be added to event: " +
                                                  string.Join(", ", eventSongResponse.Messages));
                        }
                        else
                        {
                            serviceResponse.Messages.Add("Song request created as approved and added to event playlist");
                        }
                    }
                    else
                    {
                        serviceResponse.Messages.Add("Song request created as approved (song already in event playlist)");
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }
            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = guestSongRequest.RequestID;
            return serviceResponse;
        }
        public async Task<ServiceResponse> UpdateGuestSongRequest(GuestSongRequestDTO guestSongRequestDTO)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                if (guestSongRequestDTO.RequestID <= 0)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("RequestID must be a valid positive integer");
                    return serviceResponse;
                }

                // Get the existing request directly from the database
                var existingRequest = await _context.GuestSongRequests
                    .FirstOrDefaultAsync(r => r.RequestID == guestSongRequestDTO.RequestID);

                if (existingRequest == null)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add($"Request with ID {guestSongRequestDTO.RequestID} not found");
                    return serviceResponse;
                }

                // Check if status is changing to "Approved"
                bool isNewlyApproved = existingRequest.Status != "Approved" &&
                                       guestSongRequestDTO.Status == "Approved";

                // Update only the necessary properties
                existingRequest.Status = guestSongRequestDTO.Status;

                // Save changes first
                int result = await _context.SaveChangesAsync();

                if (result <= 0)
                {
                    serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                    serviceResponse.Messages.Add("Failed to update the status. No rows affected.");
                    return serviceResponse;
                }

                // If status changed to "Approved", add to EventSong table
                if (isNewlyApproved)
                {
                    // Check if this song is already in the event
                    var existingEventSong = await _context.EventSongs
                        .FirstOrDefaultAsync(es => es.EventId == existingRequest.EventID &&
                                                   es.SongId == existingRequest.SongID);

                    if (existingEventSong == null)
                    {
                        // Add the song to the event
                        var eventSongDTO = new EventSongDTO
                        {
                            EventId = existingRequest.EventID,
                            SongId = existingRequest.SongID
                        };

                        var eventSongResponse = await _eventSongService.AddEventSong(eventSongDTO);
                        if (eventSongResponse.Status != ServiceResponse.ServiceStatus.Created)
                        {
                            // Log error but don't fail the request
                            serviceResponse.Messages.Add("Song request was approved but could not be added to event: " +
                                                     string.Join(", ", eventSongResponse.Messages));
                        }
                        else
                        {
                            serviceResponse.Messages.Add("Song request approved and added to event playlist");
                        }
                    }
                    else
                    {
                        serviceResponse.Messages.Add("Song request approved (song already in event playlist)");
                    }
                }

                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
                return serviceResponse;
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"Concurrency error: {dbEx.Message}");
                return serviceResponse;
            }
            catch (DbUpdateException dbEx)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"Database update error: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    serviceResponse.Messages.Add($"Inner exception: {dbEx.InnerException.Message}");
                }
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add($"Error updating request: {ex.Message}");
                if (ex.InnerException != null)
                {
                    serviceResponse.Messages.Add($"Inner exception: {ex.InnerException.Message}");
                }
                return serviceResponse;
            }
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