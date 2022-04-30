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

        public IActionResult Index(AllMovieQueryModel query)
        {
            const int moviesPerPage = 4;
            if (query.SearchTerm != null)
            {
                var movies = this.movies.All(query.CurrentPage, moviesPerPage, query.SearchTerm);

                query.Genres = movies.Genres;
                query.Qualities = movies.Qualities;
                query.TotalMovies = movies.TotalMovies; 
                query.Movies = movies.Movies;
            }
            else
            {
                var movies = this.movies.Index();

                query.Genres = movies.Genres;
                query.Qualities = movies.Qualities;
                query.TotalMovies = movies.TotalMovies;
                query.Movies = movies.Movies;

            }


            return View(query);
        }

        public IActionResult About() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
