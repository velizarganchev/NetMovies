using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMovies.Services.Movies.Models
{
    public class MovieDetailsServiceModel : MovieServiceModel
    {
        public string Directors { get; init; }

        public string Actors { get; init; }

        public int Duration { get; init; }

        public int GenreId { get; init; }

        public int QualityId { get; set; }

        public string CreatorId { get; set; }
    }
}
