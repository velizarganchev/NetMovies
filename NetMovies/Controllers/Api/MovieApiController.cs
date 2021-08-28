namespace NetMovies.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    using NetMovies.Services.Movies.Models;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/movies")]
    public class MovieApiController : ControllerBase
    {
        private readonly IMovieService movies;

        public MovieApiController(IMovieService movies)
        {
            this.movies = movies;
        }

        [HttpGet]
        public IEnumerable<MovieServiceModel> AllMovies()
        {
            return this.movies.AllApiMovies();
        }
    }
}
