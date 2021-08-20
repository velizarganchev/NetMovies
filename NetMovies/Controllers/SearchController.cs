namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class SearchController : Controller
    {
        private readonly IMovieService movies;

        public SearchController(IMovieService movies)
        {
            this.movies = movies;
        }

        public IActionResult Search([FromQuery] AllMovieQueryModel query) 
        {

            var queryResult = this.movies.All(              
               AllMovieQueryModel.MoviesPerPage,
               query.CurrentPage,
               query.SearchTerm);

            query.Movies = queryResult.Movies;
            query.TotalMovies = queryResult.TotalMovies;

            return View(query);
        } 
    }
}
