namespace NetMovies.Services.Movies
{
    using System.Collections.Generic;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies.Models;
 
    public interface IMovieService
    {
        MovieQueryServiceModel Index();

        MovieQueryServiceModel All(
            int currentPage,
            int moviesPerPage,
            string searchTerm);

        public IEnumerable<MovieServiceModel> AllApiMovies();

        int Create(string[] directorNames, string creatorId,string title,
            int year, string imageUrl, string watchUrl,string country, int duration,
            string descriptions, int genreId, List<string> actors);

        public bool Edit(int id, string[] directorNames, string creatorId,
            string title, int year, string imageUrl, string watchUrl,
            string country, int duration, string descriptions, int genreId, List<string> actors);

        public MovieDetailsServiceModel Details(int id);

        public MovieQueryServiceModel AllMovies(string userId);

        public string[] DirectorNames(MovieFormModel movie);

        public List<string> ActorsList(MovieFormModel movie);

        IEnumerable<MovieGenreServiceModel> GenreCategories();

        bool GenreExists(int genreId);

        //bool MovieExists(int movieId);
    }
}
