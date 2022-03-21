namespace NetMovies.Services.Movies
{
    using System.Collections.Generic;

    using NetMovies.Models.Movie;
    using Services.Movies.Models;

    public interface IMovieService
    {
        MovieQueryServiceModel Index();

        MovieQueryServiceModel All(
            int currentPage,
            int moviesPerPage,
            string searchTerm);

        public IEnumerable<MovieServiceModel> AllApiMovies();

        public IEnumerable<MovieServiceModel> MyApiMovies(string userId);

        int Create(List<string> directorNames, string creatorId, MovieFormModel movie, List<string> actors);

        public bool Edit(int id, List<string> directorNames, string creatorId, MovieFormModel movie, List<string> actors);

        public MovieDetailsServiceModel Details(int id);

        public MovieQueryServiceModel MyMovies(
            string userId,
            int currentPage,
            int moviesPerPage);

        public List<string> DirectorsList(MovieFormModel movie);

        public List<string> ActorsList(MovieFormModel movie);

        public IEnumerable<MovieGenreServiceModel> GenreCategories();

        public IEnumerable<MovieQualityServiceModel> Qualities();

        bool GenreExists(int genreId);

        public bool Delete(int id);
    }
}
