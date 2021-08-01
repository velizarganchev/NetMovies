using NetMovies.Data;

namespace NetMovies.Models.Movie
{   
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    public class AddMovieFormModel
    {
        [Required]
        [StringLength(MovieTitleMaxLenght,MinimumLength = MovieTitleMinLenght)]
        public string Title { get; init; }

        [Range(MovieYearMinValue,MovieYearMaxValue)]
        public int Year { get; init; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Required]
        [StringLength(MovieCountryMaxLenght, MinimumLength = MovieCountryMinLenght)]
        public string Country { get; init; }

        [Required]
        [RegularExpression(@"^((?:[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+, )*)[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+$")]
        public string Director { get; init; } 

        [Required]
        //[RegularExpression(@"^((?:[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+, )*)[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+$")]
        public string Actors { get; init; }

        [Range(MovieDurationMinValue,MovieDurationMaxValue)]
        public int Duration { get; init; }

        [Required]
        [MinLength(MovieDescriptionsMinLenght)]
        public string Descriptions { get; init; }

        //??????????????
        [Display(Name = "Genre")]
        [Required]
        public int GenreId { get; init; }

        public IEnumerable<MovieGenreViewModel> Genres { get; set; }
    }
}
