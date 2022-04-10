namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data.Models;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class WatchListController : Controller
    {
        private readonly IMovieService movies;
        private readonly UserManager<AppUsers> userManager;
        public WatchListController(
            IMovieService movies,
            UserManager<AppUsers> userManager)
        {
            this.movies = movies;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult MovieList(AllMovieQueryModel query, int id = 1)
        {
            var movies = this.movies.MyListMovies(this.User.Id(), query.CurrentPage = id, query.MoviesPerPage);

            query.Movies = movies.Movies;
            query.TotalMovies = movies.TotalMovies;

            return View(query);
        }

        [Authorize]
        public  IActionResult Remove(int id)
        {
            var movieForDeletet = this.movies.Remove(id, this.User.Id());

            if (movieForDeletet)
            {
                return RedirectToAction(nameof(MovieList));
            }

            return BadRequest();
        }
    }
}
