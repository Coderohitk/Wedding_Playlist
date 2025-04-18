using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Wedding_Playlist.Controllers
{
    public class EventPageController : BaseController
    {
        private readonly IEventService _eventService;
        private readonly IGuestService _guestService;
        private readonly IEventGuestService _eventGuestService;
        private readonly IEventSongService _eventSongService;
        private readonly ISongService _songService;

        public EventPageController(
            IEventService eventService,
            IGuestService guestService,
            IEventGuestService eventGuestService,
            IEventSongService eventSongService,
            ISongService songService,
            IDashboardService dashboardService)
            : base(dashboardService)
        {
            _eventService = eventService;
            _guestService = guestService;
            _eventGuestService = eventGuestService;
            _eventSongService = eventSongService;
            _songService = songService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("ListEvent");
        }

        public async Task<IActionResult> ListEvent()
        {
            var events = await _eventService.GetEvents();
            var eventDTO = events.Select(x => new EventDTO
            {
                EventId = x.EventId,
                Name = x.Name,
                Date = x.Date,
                Location = x.Location
            });

            return View(eventDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var events = await _eventService.GetEventById(id);
            var eventGuests = await _eventGuestService.GetEventGuestsbyEventId(id);
            var eventSongs = await _eventSongService.GetEventSongbyEventId(id);

            List<Guest> guestList = new List<Guest>();
            foreach (var item in eventGuests)
            {
                var guest = await _guestService.GetGuestById(item.GuestId);
                guestList.Add(guest);
            }

            List<Song> songList = new List<Song>();
            foreach (var song in eventSongs)
            {
                var songDetails = await _songService.GetSong(song.SongId);
                songList.Add(songDetails);
            }

            ViewData["EventName"] = events.Name;
            ViewData["Guests"] = guestList;
            ViewData["Songs"] = songList;

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventDTO eventDTO)
        {
            var response = await _eventService.AddEvent(eventDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("ListEvent", "EventPage");
            }

            return RedirectToAction("Create", "EventPage");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var eventDTO = await _eventService.GetEventById(id);
            if (eventDTO == null) return NotFound();

            return View(new EventDTO
            {
                EventId = eventDTO.EventId,
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventDTO eventDTO)
        {
            var response = await _eventService.UpdateEvent(eventDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("ListEvent", "EventPage");
            }

            return View(eventDTO);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var eventDTO = await _eventService.GetEventById(id);
            if (eventDTO == null) return NotFound();

            return View(eventDTO);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var response = await _eventService.DeleteEvent(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("ListEvent", "EventPage");
            }

            return RedirectToAction("Delete", "EventPage");
        }
    }
}
