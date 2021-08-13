using NetMovies.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetMovies.Models.Movie
{
    public class AllMovieQueryModel
    {
        public const int MoviesPerPage = 3;

        public string SearchTerm { get; init; }

        public int TotalMovies { get; set; }

        public int CurrentPage { get; set; } = 1;

        public IEnumerable<MovieListingViewModel> Movies { get; set; }
    }
}
