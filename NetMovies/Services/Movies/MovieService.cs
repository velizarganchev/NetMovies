namespace NetMovies.Services.Movies
{
    using NetMovies.Data;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public class MovieService : IMovieService
    {
        private readonly NetMoviesDbContext data;
        private readonly IConfigurationProvider mapper;

        public MovieService(NetMoviesDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public MovieQueryServiceModel All(
            int moviesPerPage = int.MaxValue,
            int currentPage = 1,
            string searchTerm = null)
        {

            var movieQuery = this.data.Movies.AsQueryable();

            var moviesTest = this.data.Movies.ToList();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                movieQuery = movieQuery.Where(m =>
                m.Title.ToLower().Contains(searchTerm) ||
                m.Genre.GenreName.ToLower().Contains(searchTerm) ||
                m.Actors.FirstOrDefault(a => a.FullName == searchTerm).FullName == searchTerm);
            }

            var totalMovies = this.data.Movies.Count();

            var movies = movieQuery
                .Skip((currentPage - 1) * AllMovieQueryModel.MoviesPerPage)
                .Take(AllMovieQueryModel.MoviesPerPage)
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieServiceModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Country = m.Country
                }).ToList();

            return new MovieQueryServiceModel
            {                
                TotalMovies = totalMovies,
                CurrentPage = currentPage,
                MoviesPerPage = moviesPerPage,
                Movies = movies
            };
        }

        private IEnumerable<MovieServiceModel> GetMovies(IQueryable<Movie> movieQuery)
            => movieQuery
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();
    }
}
