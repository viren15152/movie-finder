using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieFinder.Services
{
    public class TmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TmdbService(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _apiKey = config["TMDB_API_KEY"] ?? throw new Exception("TMDB API Key is missing in .env or appsettings.json");
        }

        public async Task<string> SearchMovie(string query)
        {
            var url = $"https://api.themoviedb.org/3/search/movie?query={query}&api_key={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }
    }
}

