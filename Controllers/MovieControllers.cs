using Microsoft.AspNetCore.Mvc;
using MovieFinder.Services;
using MovieFinder.Data;
using MovieFinder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("Movie")]
    public class MovieController : Controller
    {
        private readonly TmdbService _tmdbService;
        private readonly AppDbContext _context;

        // Constructor
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
            using var jsonDoc = System.Text.Json.JsonDocument.Parse(response);
            var root = jsonDoc.RootElement;

            var results = root.GetProperty("results").EnumerateArray()
                .Select(movie =>
                {
                    var movieDetails = new Dictionary<string, object>
                    {
                        { "title", movie.GetProperty("title").GetString() ?? "Unknown Title" },  // Default to "Unknown Title" if null
                        { "year", movie.TryGetProperty("release_date", out var date) ? date.GetString()?.Split('-')[0] ?? "N/A" : "N/A" },  // Default to "N/A" if null
                        { "image", movie.TryGetProperty("poster_path", out var poster) ? $"https://image.tmdb.org/t/p/w500{poster.GetString()}" : "https://via.placeholder.com/100" },  // Default to placeholder if null
                        { "tmdbId", movie.GetProperty("id").GetInt32().ToString() ?? "Unknown ID" }  // Default to "Unknown ID" if null
                    };

                    return movieDetails;
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

        // 🎬 ✅ Remove a Movie from Favorites (POST)
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







