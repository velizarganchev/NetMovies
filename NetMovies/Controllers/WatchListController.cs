namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Models.Movie;

    public class WatchListController : Controller
    {
        private readonly NetMoviesDbContext data;

        public WatchListController(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public IActionResult MovieList(AllMovieQueryModel query, int id = 1)
        {
            //var movies = this.movies.MyMovies(this.User.Id(), query.CurrentPage = id, query.MoviesPerPage);

            //query.Movies = movies.Movies;
            query.TotalMovies = 0;

            return View(query);
        }
    }
}
