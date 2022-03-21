
namespace NetMovies.Services.Movies.Models
{
    using NetMovies.Data.Models;
    using System.Collections.Generic;

    public class MovieEditServiceModel : MovieServiceModel
    {
        public IEnumerable<Director> Directors { get; init; }

        public IEnumerable<Actor> Actors { get; init; }

        public int Duration { get; init; }

        public int GenreId { get; init; }

        public int QualityId { get; set; }

        public string CreatorId { get; set; }
    }
}
