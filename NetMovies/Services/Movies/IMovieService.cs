using NetMovies.Models.Movie;

namespace NetMovies.Services.Movies
{
    public interface IMovieService
    {
        MovieQueryServiceModel All(
            int moviesPerPage = int.MaxValue,
            int currentPage = 1,
            string searchTerm = null);
    }
}
