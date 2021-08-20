namespace NetMovies.Services.Movies.Models
{
    public class MovieServiceModel
    {
        public int MovieId { get; init; }

        public string Title { get; init; }

        public int Year { get; init; }

        public string ImageUrl { get; init; }

        public string Country { get; init; }
    }
    // Замества MovieListingViewModel, да го променя на всякъде
}
