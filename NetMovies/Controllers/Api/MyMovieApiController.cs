namespace NetMovies.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data.Models;
    using NetMovies.Infrastructure.Extensions;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    using NetMovies.Services.Movies.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/mymovies")]
    public class MyMovieApiController : ControllerBase
    {
        private readonly IMovieService movies;
        private readonly UserManager<AppUsers> userManager;

        public MyMovieApiController(
            IMovieService movies,
            UserManager<AppUsers> userManager)
        {
            this.movies = movies;
            this.userManager = userManager;
        }

        //public IEnumerable<MovieServiceModel> MyMovies()
        //{          
        //    return this.movies.MyApiMovies(this.User.Id());
        //}

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AddInMyListResponseModel>> Post(AddToMyListInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var userId = this.movies.AddUserToMovie(input.movieId, user);

            return new AddInMyListResponseModel { UserId = userId, };
        }
    }
}
