namespace NetMovies.Services.Movies
{
    using NetMovies.Data;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies.Models;
    using NetMovies.Services.Statistics;
    using System.Collections.Generic;
    using System.Linq;

    public class MovieService : IMovieService
    {
        private readonly NetMoviesDbContext data;
        private readonly IStatisticService statistics;

        public MovieService(NetMoviesDbContext data, IStatisticService statistics)
        {
            this.data = data;
            this.statistics = statistics;
        }

        public MovieQueryServiceModel Index()
        {
            var movies = this.data
            .Movies
            .OrderByDescending(m => m.MovieId)
            .Select(m => new MovieServiceModel
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

            return new MovieQueryServiceModel
            {
                TotalMovies = totalStatistics.TotalMovies,
                Movies = movies
            };
        }

        public MovieQueryServiceModel All(
            int currentPage,
            int moviesPerPage,
            string searchTerm)
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
                .Skip((currentPage - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieServiceModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Genre = m.Genre.GenreName,
                    Country = m.Country
                }).ToList();

            var totalStatistics = this.statistics.Total();

            return new MovieQueryServiceModel
            {
                TotalMovies = totalStatistics.TotalMovies,
                Movies = movies
            };
        }

        public int Create(string[] directorNames, string creatorId,
            string title, int year, string imageUrl, string watchUrl,
            string country, int duration, string descriptions, int genreId, List<string> actors)
        {
            var director = new Director
            {
                FirstName = directorNames[0],
                LastName = directorNames[1]
            };
            var existDirector = this.data
                .Directors
                .FirstOrDefault(x => x.FirstName == director.FirstName && x.LastName == director.LastName);

            if (!this.data.Directors.Contains(existDirector))
            {
                this.data.Directors.Add(director);
                this.data.SaveChanges();
            }
            else
            {
                director.DirectorId = existDirector.DirectorId;
            }
            var movieData = new Movie
            {
                CreatorId = creatorId,
                Title = title,
                DirectorId = director.DirectorId,
                Year = year,
                ImageUrl = imageUrl,
                WatchUrl = watchUrl,
                Country = country,
                Duration = duration,
                Descriptions = descriptions,
                GenreId = genreId
            };
            var actorsList = actors;
            foreach (var actor in actorsList)
            {
                var currActor = new Actor
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1],
                    FullName = actor.Split(" ")[0] + " " + actor.Split(" ")[1]
                };

                var existActor = this.data.Actors.FirstOrDefault(x => x.FirstName == currActor.FirstName
                && x.LastName == currActor.LastName);

                if (this.data.Actors.Contains(existActor))
                {
                    movieData.Actors.Add(existActor);
                    continue;
                }
                movieData.Actors.Add(currActor);
            }

            this.data.Movies.Add(movieData);
            this.data.SaveChanges();

            return movieData.MovieId;
        }

        //public bool Edit(string[] directorNames, string creatorId,
        //    string title, int year, string imageUrl, string watchUrl,
        //    string country, int duration, string descriptions, int genreId, List<string> actors) 
        //{
        //    var movie = this.data.Movies
        //}

        public MovieQueryServiceModel MyAllMovies(string userId)
        {
            var moviesQuery = this.data.Movies.Where(m => m.CreatorId == userId).AsQueryable();

            var movies = moviesQuery
                .OrderByDescending(m => m.MovieId)
                .Select(m => new MovieServiceModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    Country = m.Country
                }).ToList();

            return new MovieQueryServiceModel { Movies = movies };
        }

        public string[] DirectorNames(AddMovieFormModel movie)
        {
            return movie.Director.Split(" ");
        }

        public List<string> ActorsList(AddMovieFormModel movie)
        {
            return movie.Director.Split(" ").ToList();
        }

        public IEnumerable<MovieGenreServiceModel> GenreCategories()
        =>
            data.Genres.Select(g => new MovieGenreServiceModel
            {
                GenreId = g.GenreId,
                Name = g.GenreName
            })
            .ToList();



        public bool GenreExists(int genreId)
             => this.data.Genres.Any(g => g.GenreId == genreId);

        public bool MovieExists(int movieId)
            => this.data.Movies.Any(m => m.MovieId == movieId);
    }
}
