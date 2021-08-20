namespace NetMovies.Models.Movie
{
    using NetMovies.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class MovieQueryServiceModel
    {
        public int MoviesPerPage { get; set; }

        public int TotalMovies { get; init; }

        public int CurrentPage { get; init; } = 1;

        public IEnumerable<MovieServiceModel> Movies { get; set; }
    }
}
