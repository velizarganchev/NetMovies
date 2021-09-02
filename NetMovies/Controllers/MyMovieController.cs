namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class MyMovieController : Controller
    {
        private readonly IMovieService movies;

        public MyMovieController(IMovieService movies) => this.movies = movies;

        [Authorize]
        public IActionResult MyAllMovies([FromQuery] AllMovieQueryModel query)
        {
            var movies = this.movies.AllMovies(this.User.Id());
            query.Movies = movies.Movies;

            return View(query);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var movie = movies.Details(id);

            return View(new MovieFormModel
            {
                Title = movie.Title,
                Year = movie.Year,
                ImageUrl = movie.ImageUrl,
                WatchUrl = movie.WatchUrl,
                Country = movie.Country,
                Director = movie.Director,
                Actors = movie.Actors,
                Duration = movie.Duration,
                Descriptions = movie.Descriptions,
                GenreId = movie.GenreId,
                Genres = this.movies.GenreCategories()
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, MovieFormModel movie)
        {
            if (!movies.GenreExists(movie.GenreId))
            {
                this.ModelState.AddModelError(nameof(movie.GenreId), "Genre does not exist.");
            }
            if (!ModelState.IsValid)
            {
                movie.Genres = this.movies.GenreCategories();

                return View(movie);
            }
            var directorNames = movies.DirectorNames(movie);
            var actorsList = movies.ActorsList(movie);

            var movieId = this.movies.Edit(id, directorNames, this.User.Id(), movie.Title, movie.Year, movie.ImageUrl,
                movie.WatchUrl, movie.Country, movie.Duration, movie.Descriptions, movie.GenreId, actorsList);

            return RedirectToAction(nameof(MyAllMovies));
        }
    }
}
