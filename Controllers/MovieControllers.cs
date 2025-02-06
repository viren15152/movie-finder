using Microsoft.AspNetCore.Mvc;
using MovieFinder.Services;  
using MovieFinder.Data;      
using MovieFinder.Models;    
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("Movie")]  // 
    public class MovieController : Controller
    {
        private readonly TmdbService _tmdbService;
        private readonly AppDbContext _context;

        public MovieController(TmdbService tmdbService, AppDbContext context)
        {
            _tmdbService = tmdbService;
            _context = context;
        }

        // 🎬 ✅ Movie Search (GET)
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Dictionary<string, object>>());
            }

            var response = await _tmdbService.SearchMovie(query);
            using var jsonDoc = JsonDocument.Parse(response);
            var root = jsonDoc.RootElement;

            var results = root.GetProperty("results").EnumerateArray()
                .Select(movie => new Dictionary<string, object>
                {
                    { "title", movie.GetProperty("title").GetString() },
                    { "year", movie.TryGetProperty("release_date", out var date) ? date.GetString()?.Split('-')[0] : "N/A" },
                    { "image", movie.TryGetProperty("poster_path", out var poster) ? $"https://image.tmdb.org/t/p/w500{poster.GetString()}" : "https://via.placeholder.com/100" },
                    { "tmdbId", movie.GetProperty("id").GetInt32().ToString() }
                }).ToList();

            return View(results);
        }

        // ⭐ ✅ Save a Movie to Favorites (POST)
        [HttpPost("SaveMovie")]
        public IActionResult SaveMovie(Movie movie)
        {
            if (movie != null && !string.IsNullOrWhiteSpace(movie.TmdbId))
            {
                var existingMovie = _context.Movies.FirstOrDefault(m => m.TmdbId == movie.TmdbId);
                if (existingMovie == null)
                {
                    _context.Movies.Add(movie);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Favorites");
        }

        // ❤️ ✅ Display Favorite Movies (GET)
        [HttpGet("Favorites")]
        public IActionResult Favorites()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        // 🎬 ✅ Remove Movie from Favorites (POST)
        [HttpPost("RemoveMovie")]
        public IActionResult RemoveMovie(string tmdbId)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.TmdbId == tmdbId);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }
            return RedirectToAction("Favorites");
        }
    }
}




