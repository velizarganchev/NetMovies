using NetMovies.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetMovies.Models.Movie
{
    public class AllMovieQueryModel
    {
        public string SearchTerm { get; init; }

        public IEnumerable<MovieListingViewModel> Movies { get; set; }
    }
}
