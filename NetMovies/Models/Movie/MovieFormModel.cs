namespace NetMovies.Models.Movie
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Services.Movies.Models;
    using static Data.DataConstants;
    public class MovieFormModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Range(1900, 2050)]
        public int Year { get; set; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name = "Watch Url")]
        [Required]
        [Url]
        public string WatchUrl { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Country { get; set; }

        [Required]
        public string Directors { get; set; }

        [Required]
        public string Actors { get; set; }

        [Range(1, 300)]
        public int Duration { get; set; }

        [Display(Name = "Age Limit")]
        [Range(1, 100)]
        public int AgeLimit { get; set; }

        [Required]
        [Range(10, 3000)]
        public string Descriptions { get; set; }

        [Display(Name = "Genre")]
        [Required]
        [Range(1, 250)]
        public int GenreId { get; set; }

        public IEnumerable<MovieGenreServiceModel> Genres { get; set; }

        [Display(Name = "Quality")]
        [Required]
        [Range(1, 10)]
        public int QualityId { get; set; }

        public IEnumerable<MovieQualityServiceModel> Qualities { get; set; }
    }
}
