using System.ComponentModel.DataAnnotations;

namespace MovieFinder.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string TmdbId { get; set; } = string.Empty;

        public string PosterUrl { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
    }
}


