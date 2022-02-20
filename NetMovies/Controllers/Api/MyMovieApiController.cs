namespace NetMovies.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Services.Movies;
    using NetMovies.Services.Movies.Models;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/mymovies")]
    public class MyMovieApiController : ControllerBase
    {
        private readonly IMovieService movies;
        public MyMovieApiController(IMovieService movies)
        {
            this.movies = movies;
        }

        public IEnumerable<MovieServiceModel> MyMovies()
        {          
            return this.movies.MyApiMovies(this.User.Id());
        }
    }
}
