namespace NetMovies.Models.Movie
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Services.Movies.Models;
    using static Data.DataConstants;
    public class MovieFormModel
    {
        [Required]
        [StringLength(MovieTitleMaxLenght, MinimumLength = MovieTitleMinLenght)]
        public string Title { get; set; }

        [Range(MovieYearMinValue, MovieYearMaxValue)]
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
        [StringLength(MovieCountryMaxLenght, MinimumLength = MovieCountryMinLenght)]
        public string Country { get; set; }

        [Required]
        public string Directors { get; set; }

        [Required]
        public string Actors { get; set; }

        [Range(MovieDurationMinValue, MovieDurationMaxValue)]
        public int Duration { get; set; }

        [Display(Name = "Age Limit")]
        public int AgeLimit { get; set; }

        [Required]
        [MinLength(MovieDescriptionsMinLenght)]
        public string Descriptions { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public int GenreId { get; set; }

        public IEnumerable<MovieGenreServiceModel> Genres { get; set; }

        [Display(Name = "Quality")]
        [Required]
        public int QualityId { get; set; }

        public IEnumerable<MovieQualityServiceModel> Qualities { get; set; }
    }
}
