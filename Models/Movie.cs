using System.ComponentModel.DataAnnotations;

namespace MovieFinder.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string TmdbId { get; set; } // Stores TMDB ID

        public string PosterUrl { get; set; }

        public string Year { get; set; }
    }
}

