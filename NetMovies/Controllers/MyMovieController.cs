namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using System.Linq;

    public class MyMovieController : Controller
    {
        private readonly NetMoviesDbContext data;

        public MyMovieController(NetMoviesDbContext data)
        {
            this.data = data;
        }
        [Authorize]
        public IActionResult MyAllMovies([FromQuery] AllMovieQueryModel query) 
        {
            var moviesQuery = this.data.Movies.Where(m => m.CreatorId == this.User.Id()).AsQueryable();

            var movies = moviesQuery
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieListingViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Country = m.Country
                }).ToList();
            query.Movies = movies;

            return View(query);
        }
    }
}
