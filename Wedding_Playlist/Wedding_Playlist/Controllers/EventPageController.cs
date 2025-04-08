using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding_Playlist.Data;
using Wedding_Playlist.Models;
using Wedding_Playlist.Interfaces;
namespace Wedding_Playlist.Controllers
{
    public class EventPageController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IGuestService _guestService;
        private readonly IEventGuestService _eventGuestService;
        public EventPageController(IEventService eventService, IGuestService guestService,IEventGuestService eventGuestService)
        {
            _eventService = eventService;
            _guestService = guestService;
            _eventGuestService = eventGuestService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("ListEvent");
        }
        public async Task<IActionResult> ListEvent()
        {
            var events = await _eventService.GetEvents();
            var eventDTO = events.Select(x => new EventDTO()
            {
                EventId = x.EventId,
                Name = x.Name,
                Date = x.Date,
                Location = x.Location,


            }
            );
            return View(eventDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var events = await _eventService.GetEventById(id);
            var eventguest = await _eventGuestService.GetEventGuestsbyEventId(id);
            List<Guest> guestlist = new List<Guest>();
            foreach(var item in eventguest)
            {
                var guest = await _guestService.GetGuestById(item.GuestId);
                guestlist.Add(guest);
            }
            ViewData["EventName"] = events.Name;
            ViewData["Guests"] = guestlist;
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
            else
            {
                return RedirectToAction("Create", "EventPage");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventDTO = await _eventService.GetEventById(id);
            if (eventDTO == null)
            {
                return NotFound();
            }
            var model = new EventDTO()
            {
                EventId = eventDTO.EventId,
                Name = eventDTO.Name,
                Date = eventDTO.Date,
                Location = eventDTO.Location
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventDTO eventDTO)
        {
            var response = await _eventService.UpdateEvent(eventDTO);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                // ? Go back to the list page after a successful update
                return RedirectToAction("ListEvent", "EventPage");
            }

            // ? If something went wrong, return the same view with the current model
            return View(eventDTO);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventDTO = await _eventService.GetEventById(id);
            if (eventDTO == null)
            {
                return NotFound();
            }

            return View(eventDTO); // This will render your delete confirmation page
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var response = await _eventService.DeleteEvent(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("ListEvent", "EventPage");
            }
            else
            {
                return RedirectToAction("Delete", "EventPage");
            }

        }
    }
}


