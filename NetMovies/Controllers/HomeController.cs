namespace NetMovies.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Models;
    using NetMovies.Models.Home;
    using NetMovies.Services.Statistics;

    public class HomeController : Controller
    {
        private readonly NetMoviesDbContext data;
        private readonly IStatisticService statistics;
        public HomeController(
            NetMoviesDbContext data,
            IStatisticService statistics)
        {
            this.data = data;
            this.statistics = statistics;
        }
        public IActionResult Index()
        {
            var movies = this.data
                .Movies
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieIndexViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Genre = m.Genre.GenreName,
                    Country = m.Country
                })
                .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel 
            {
                TotalMovies = totalStatistics.TotalMovies,
                Movies = movies
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
