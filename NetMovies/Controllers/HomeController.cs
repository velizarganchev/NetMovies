namespace NetMovies.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Models;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    public class HomeController : Controller
    {
        private readonly IMovieService movies;

        public HomeController(
            IMovieService movies)
        {
            this.movies = movies;
        }
        public IActionResult Index([FromQuery] AllMovieQueryModel query)
        {
            var movies = this.movies.Index();

            query.TotalMovies = movies.TotalMovies;
            query.Movies = movies.Movies;

            return View(query);
        }
            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
