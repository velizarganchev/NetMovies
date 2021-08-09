namespace NetMovies.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Models.Movie;
    using System.Linq;

    public class SearchController : Controller
    {
        private readonly NetMoviesDbContext data;

        public SearchController(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public IActionResult Search(string searchTerm) 
        {
            var movisQuery = this.data.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                movisQuery = movisQuery.Where(m =>
                m.Title.ToLower().Contains(searchTerm) ||
                m.Genre.GenreName.ToLower().Contains(searchTerm) ||
                m.Actors.FirstOrDefault(a => a.FullName == searchTerm).FullName == searchTerm); 
            }

            var movies = movisQuery
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieListingViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Country = m.Country
                }).ToList();

            return View(new AllMovieQueryModel
            {
                Movies = movies,
                SearchTerm = searchTerm
            });

        } 
    }
}
