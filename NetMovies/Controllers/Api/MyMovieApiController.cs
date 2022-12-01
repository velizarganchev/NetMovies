namespace NetMovies.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AddInMyListResponseModel>> MyMovies(AddToMyListInputModel input)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var userId = this.movies.AddUserToMovie(input.movieId, user);

            return new AddInMyListResponseModel { UserId = userId, };
        }
    }
}
