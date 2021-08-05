namespace NetMovies.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Models;
    using NetMovies.Models.Home;

    public class HomeController : Controller
    {
        private readonly NetMoviesDbContext data;

        public HomeController(NetMoviesDbContext data)
        {
            this.data = data;
        }
        public IActionResult Index()
        {
            var totalmovies = this.data.Movies.Count();

            var movies = this.data
                .Movies
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieIndexViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Country = m.Country
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel 
            {
                TotalMovies = totalmovies,
                Movies = movies
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
