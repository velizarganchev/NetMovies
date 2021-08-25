using System.Collections.Generic;

namespace NetMovies.Services.Movies.Models
{
    public class MovieServiceFormModel
    {
        
        public string Title { get; init; }

        public int Year { get; init; }


        public string ImageUrl { get; init; }


        public string WatchUrl { get; init; }


        public string Country { get; init; }


        public string Director { get; init; }


        public string Actors { get; init; }


        public int Duration { get; init; }


        public string Descriptions { get; init; }


        public int GenreId { get; init; }

        public IEnumerable<MovieGenreServiceModel> Genres { get; set; }
    }
}
