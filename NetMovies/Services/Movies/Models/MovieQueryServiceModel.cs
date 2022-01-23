namespace NetMovies.Services.Movies.Models
{
    using System.Collections.Generic;
    public class MovieQueryServiceModel
    {
        public int MoviesPerPage { get; set; }

        public string SearchTerm { get; init; }

        public int TotalMovies { get; set; }

        public int CurrentPage { get; set; } = 1;
        public IEnumerable<MovieGenreServiceModel> Genres { get; set; }

        public IEnumerable<MovieQualityServiceModel> Qualities { get; set; }

        public IEnumerable<MovieServiceModel> Movies { get; set; }
    }
}
