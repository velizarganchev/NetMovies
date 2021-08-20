namespace NetMovies.Models.Movie
{
    using NetMovies.Services.Movies.Models;

    public class MovieServiceModel : IMovieModel
    {
        public int MovieId { get; init; }

        public string Title { get; init; }

        public int Year { get; init; }

        public string ImageUrl { get; init; }

        public string Country { get; init; }

        public string CreatorId { get; init; }
    }
}
