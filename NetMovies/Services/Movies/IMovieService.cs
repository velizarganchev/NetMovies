using NetMovies.Services.Movies.Models;

namespace NetMovies.Services.Movies
{
  public  interface IMovieService
    {
        MovieQueryServiceModel All(
            int currentPage,
            int moviesPerPage,
            string searchTerm);
    }
}
