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

        public IActionResult Search([FromQuery] AllMovieQueryModel query) 
        {
            var movisQuery = this.data.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                movisQuery = movisQuery.Where(m =>
                m.Title.ToLower().Contains(query.SearchTerm) ||
                m.Genre.GenreName.ToLower().Contains(query.SearchTerm) ||
                m.Actors.FirstOrDefault(a => a.FullName == query.SearchTerm).FullName == query.SearchTerm); 
            }

            var movies = movisQuery
                .Skip((query.CurrentPage - 1) * AllMovieQueryModel.MoviesPerPage)
                .Take(AllMovieQueryModel.MoviesPerPage)
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieListingViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Country = m.Country
                }).ToList();

            query.Movies = movies;

            return View(query);
        } 
    }
}
