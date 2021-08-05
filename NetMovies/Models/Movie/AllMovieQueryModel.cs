using NetMovies.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetMovies.Models.Movie
{
    public class AllMovieQueryModel
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public string ImageUrl { get; set; }
  
        public string Country { get; set; }
    }
}
