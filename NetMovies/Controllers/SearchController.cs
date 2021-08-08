namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;

    public class SearchController : Controller
    {
        private readonly NetMoviesDbContext data;

        public SearchController(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public IActionResult Search() => View(); 
    }
}
