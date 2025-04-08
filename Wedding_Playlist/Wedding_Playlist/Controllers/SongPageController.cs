using CoreEntityFramework.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Controllers
{
    public class SongPageController : Controller
    {
        private readonly ISongService _songService;
        private readonly IEventService _eventService;
        private readonly IPlaylistService _playlistService;
        private readonly IEventSongService _eventSongService;
        private readonly IPlaylistSongService _playlistSongService;

        public SongPageController(ISongService songService, IEventService eventService, IPlaylistService playlistService, IEventSongService eventSongService, IPlaylistSongService playlistSongService)
        {
            _songService = songService;
            _eventService = eventService;
            _playlistService = playlistService;
            _eventSongService = eventSongService;
            _playlistSongService = playlistSongService;
        }

        public async Task<IActionResult> Index()
        {
            var songs = await _songService.GetAllSongs();
            var dtoList = songs.Select(s => new SongDTO
            {
                SongId = s.SongId,
                Title = s.Title,
                Artist = s.Artist,
                Genre = s.Genre,
                Description = s.Description
            });
            return View(dtoList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var eventList = await _eventService.GetEvents();
            var playlistList = await _playlistService.GetAllPlaylists();

            var viewModel = new SongCreateViewModel
            {
                Song = new SongDTO(),
                EventSelections = eventList.Select(e => new EventSelection { EventId = e.EventId, IsSelected = false }).ToList(),
                PlaylistSelections = playlistList.Select(p => new PlaylistSelection { PlaylistID = p.PlaylistID, IsSelected = false }).ToList()
            };

            ViewData["EventsList"] = eventList.ToList();
            ViewData["Playlist"] = playlistList.ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongCreateViewModel viewModel, IFormCollection form)
        {
            if (!ModelState.IsValid)
            {
                var eventList = await _eventService.GetEvents();
                var playlistList = await _playlistService.GetAllPlaylists();
                ViewData["EventsList"] = eventList.ToList();
                ViewData["Playlist"] = playlistList.ToList();
                return View(viewModel);
            }

            // Create the song
            var songResponse = await _songService.CreateSong(viewModel.Song);
            if (songResponse.Status != ServiceResponse.ServiceStatus.Created)
            {
                TempData["Error"] = string.Join("; ", songResponse.Messages);
                return View(viewModel);
            }

            int songId = songResponse.CreatedId;

            // Process event selections from form data
            string[] eventSelections = form["EventSelections"].ToArray();
            foreach (var eventIdStr in eventSelections)
            {
                if (!string.IsNullOrEmpty(eventIdStr) && int.TryParse(eventIdStr, out int eventId))
                {
                    var eventSongDTO = new EventSongDTO
                    {
                        EventId = eventId,
                        SongId = songId
                    };

                    await _eventSongService.AddEventSong(eventSongDTO);
                }
            }

            // Process playlist selections from form data
            string[] playlistSelections = form["PlaylistSelections"].ToArray();
            foreach (var playlistIdStr in playlistSelections)
            {
                if (!string.IsNullOrEmpty(playlistIdStr) && int.TryParse(playlistIdStr, out int playlistId))
                {
                    var playlistSongDTO = new PlaylistSongDTO
                    {
                        PlaylistID = playlistId,
                        SongID = songId
                    };

                    await _playlistSongService.CreatePlaylistSong(playlistSongDTO);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _songService.GetSong(id);
            if (song == null)
                return NotFound();

            var songDTO = new SongDTO
            {
                SongId = song.SongId,
                Title = song.Title,
                Artist = song.Artist,
                Genre = song.Genre,
                Description = song.Description
            };

            return View(songDTO);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongDTO songDTO)
        {
            if (id != songDTO.SongId)
                return BadRequest();

            var response = await _songService.UpdateSong(songDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = string.Join("; ", response.Messages);
            return View(songDTO);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _songService.GetSong(id);
            if (song == null)
                return NotFound();

            // Convert Song to SongDTO
            var songDTO = new SongDTO
            {
                SongId = song.SongId,
                Title = song.Title,
                Artist = song.Artist,
                Genre = song.Genre,
                Description = song.Description
            };

            return View(songDTO);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _songService.DeleteSong(id);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = string.Join("; ", response.Messages);
            return RedirectToAction("Delete", new { id });
        }
    }
}
