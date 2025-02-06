using Microsoft.AspNetCore.Mvc;
using MovieFinder.Services;  // ✅ Corrected namespace
using MovieFinder.Data;      // ✅ Corrected namespace
using MovieFinder.Models;    // ✅ Corrected namespace
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    public class MovieController : Controller
    {
        private readonly TmdbService _tmdbService;
        private readonly AppDbContext _context;

        public MovieController(TmdbService tmdbService, AppDbContext context)
        {
            _tmdbService = tmdbService;
            _context = context;
        }

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
    }
}


