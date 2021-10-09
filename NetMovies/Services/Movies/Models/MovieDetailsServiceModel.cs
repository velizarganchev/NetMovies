using NetMovies.Data.Models;
using System.Collections.Generic;

namespace NetMovies.Services.Movies.Models
{
    public class MovieDetailsServiceModel : MovieServiceModel
    {
        public string WatchUrl { get; init; }

        public int DirectorId { get; init; }

        public string Directors { get; init; }

        public string Actors { get; init; }

        public int Duration { get; init; }

        public string Descriptions { get; init; }

        public int GenreId { get; init; }

        public string CreatorId { get; set; }

    }
}
