namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;

    public class MovieController : Controller
    {
        private readonly NetMoviesDbContext data;
        private readonly IMovieService movies;
        public MovieController(
            NetMoviesDbContext data,
            IMovieService movies)
        {
            this.data = data;
            this.movies = movies;
        }

        public IActionResult All([FromQuery] AllMovieQueryModel query)
        {
            var movies = this.movies.All(query.CurrentPage, AllMovieQueryModel.MoviesPerPage,query.SearchTerm);

            query.TotalMovies = movies.TotalMovies;
            query.Movies = movies.Movies;

            return View(query);
        }

        [Authorize]
        public IActionResult Add() => View(new AddMovieFormModel
        {
            Genres = this.movies.GenreCategories()

        });

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddMovieFormModel movie)
        {
            if (movies.GenreExists(movie.GenreId))
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

            var movieId = this.movies.Create(directorNames, this.User.Id(), movie.Title, movie.Year, movie.ImageUrl,
                movie.WatchUrl, movie.Country, movie.Duration, movie.Descriptions, movie.GenreId, actorsList);

            if (movies.MovieExists(movieId))
            {
                return RedirectToAction("Add", "Movie");
            }
            return RedirectToAction(nameof(All));
        }

        //[Authorize]
        //public IActionResult Edit(int id)
        //{
        //    this.movies.Edit();
        //}

    }
}
