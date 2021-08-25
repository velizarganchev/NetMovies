namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    using System.Linq;

    public class MyMovieController : Controller
    {
        private readonly IMovieService movies;

        public MyMovieController(IMovieService movies)
        {
            this.movies = movies;
        }
        [Authorize]
        public IActionResult MyAllMovies([FromQuery] AllMovieQueryModel query)
        {
            var movies = this.movies.MyAllMovies(this.User.Id());

            query.Movies = movies.Movies;

            return View(query);
        }
    }
}
