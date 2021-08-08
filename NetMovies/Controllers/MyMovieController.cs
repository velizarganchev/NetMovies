namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    public class MyMovieController : Controller
    {
        private readonly NetMoviesDbContext data;

        public MyMovieController(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public IActionResult MyAllMovies() => View();
    }
}
