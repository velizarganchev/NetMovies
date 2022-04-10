namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using NetMovies.Data.Models;

    public class MovieController : Controller
    {
        private readonly IMovieService movies;
        private readonly UserManager<AppUsers> userManager;

        public MovieController(
            IMovieService movies,
            UserManager<AppUsers> userManager)
        {
            this.movies = movies;
            this.userManager = userManager;
        }

        public IActionResult All(AllMovieQueryModel query, int id = 1)
        {
            var movies = this.movies.All(query.CurrentPage = id, query.MoviesPerPage, query.SearchTerm);

            query.Genres = movies.Genres;
            query.Qualities = movies.Qualities;
            query.TotalMovies = movies.TotalMovies;
            query.Movies = movies.Movies;

            return View(query);
        }

        [Authorize]
        public IActionResult Add() => View(new MovieFormModel
        {
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

            var movieId = this.movies.Create(directorsList, this.User.Id(), movie, actorsList);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult MovieDetails(int id) => View(this.movies.Details(id, this.User.Id()));

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> MovieDetails(MovieMyWatchlistModel model)
        //{
        //    var user = await this.userManager.GetUserAsync(this.User);

        //    return this.Json(user);
        //    var addMovie = movies.AddMovieInMyList(model.movieId, user);
        //    return View();
        //}
    }

}

