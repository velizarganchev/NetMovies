namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class SearchController : Controller
    {
        private readonly IMovieService movies;
        public SearchController(IMovieService movies)
        {
            this.movies = movies;
        }

        public IActionResult Search(AllMovieQueryModel query)
        {
            var movies = this.movies.All(query.CurrentPage, AllMovieQueryModel.MoviesPerPage, query.SearchTerm);

            query.TotalMovies = movies.TotalMovies;
            query.Movies = movies.Movies;

            return View(query);
        }
    }
}
