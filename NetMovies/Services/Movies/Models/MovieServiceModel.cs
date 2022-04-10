namespace NetMovies.Services.Movies.Models
{
    public class MovieServiceModel
    {
        public int MovieId { get; init; }

        public string Title { get; init; }

        public int Year { get; init; }

        public string ImageUrl { get; init; }

        public string WatchUrl { get; set; }

        public string Genre { get; set; }

        public string Quality { get; set; }

        public int AgeLimit { get; set; }

        public string Description { get; set; }

        public string Country { get; init; }

        public int VotesCount { get; set; }

    }
}
