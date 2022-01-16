namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class MovieController : Controller
    {
        private readonly IMovieService movies;

        public MovieController(IMovieService movies) => this.movies = movies;

        public IActionResult All([FromQuery] AllMovieQueryModel query)
        {
            var movies = this.movies.All(query.CurrentPage, AllMovieQueryModel.MoviesPerPage, query.SearchTerm);

            query.TotalMovies = movies.TotalMovies;
            query.Movies = movies.Movies;

            return View(query);
        }

        [Authorize]
        public IActionResult Add() => View(new MovieFormModel{
            Genres = this.movies.GenreCategories(),
            Qualities = this.movies.Qualities()
        });

        [HttpPost]
        [Authorize]
        public IActionResult Add(MovieFormModel movie)
        {
            if (!movies.GenreExists(movie.GenreId))
            {
                this.ModelState.AddModelError(nameof(movie.GenreId), "Genre does not exist.");
            }  

            if (!ModelState.IsValid)
            {
                movie.Genres = this.movies.GenreCategories();
                movie.Qualities = this.movies.Qualities();

                return View(movie);
            }
            var directorsList = movies.DirectorsList(movie);
            var actorsList = movies.ActorsList(movie);

            var movieId = this.movies.Create(directorsList, this.User.Id(), movie.Title, movie.Year, movie.ImageUrl,
                movie.WatchUrl, movie.Country, movie.Duration, movie.Descriptions, movie.GenreId, movie.QualityId, movie.AgeLimit , actorsList);

            return RedirectToAction(nameof(All));
        }

        public IActionResult MovieDetails(int id)
        {
            var movie = this.movies.Details(id);

            return View(movie);
        }

    }
}
