using Microsoft.AspNetCore.Mvc;
using Wedding_Playlist.Interfaces;
using Wedding_Playlist.Models;

namespace Wedding_Playlist.Controllers
{
    public class SongPageController : Controller
    {
        private readonly ISongService _songService;

        public SongPageController(ISongService songService)
        {
            _songService = songService;
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongDTO songDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(songDTO);
            }
            var response = await _songService.CreateSong(songDTO);
            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = string.Join("; ", response.Messages);
            return View(songDTO);
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
