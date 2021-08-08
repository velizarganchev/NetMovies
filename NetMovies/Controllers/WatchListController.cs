namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;

    public class WatchListController : Controller
    {
        private readonly NetMoviesDbContext data;

        public WatchListController (NetMoviesDbContext data)
        {
            this.data = data;
        }

        public IActionResult MovieList() => View();
    }
}
