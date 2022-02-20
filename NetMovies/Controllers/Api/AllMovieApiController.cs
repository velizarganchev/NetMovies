namespace NetMovies.Controllers.Api
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using NetMovies.Services.Movies;
    using NetMovies.Services.Movies.Models;

    [ApiController]
    [Route("api/allmovies")]
    public class AllMovieApiController : ControllerBase
    {
        private readonly IMovieService movies;

        public AllMovieApiController(IMovieService movies)
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
