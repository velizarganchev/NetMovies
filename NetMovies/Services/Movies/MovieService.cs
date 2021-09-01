namespace NetMovies.Services.Movies
{
    using AutoMapper;
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
        private readonly IMapper mapper;
        public MovieService(
            NetMoviesDbContext data,
            IStatisticService statistics,
            IMapper mapper)
        {
            this.data = data;
            this.statistics = statistics;
            this.mapper = mapper;
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
                m.MovieActors.FirstOrDefault(a => a.Actor.FullName == searchTerm).Actor.FullName == searchTerm);
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

        public IEnumerable<MovieServiceModel> AllApiMovies()
        {
            var movisQuery = this.data.Movies.AsQueryable();

            var movies = movisQuery.OrderBy(m => m.MovieId)
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

            return movies;
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

            foreach (var actor in actors)
            {
                var currActor = new Actor
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1],
                    FullName = actor.Split(" ")[0] + " " + actor.Split(" ")[1]
                };

                var existActor = this.data.Actors.FirstOrDefault(x => x.FirstName == currActor.FirstName
                && x.LastName == currActor.LastName);

                if (!this.data.Actors.Contains(existActor))
                {
                    this.data.Actors.Add(currActor);
                    this.data.SaveChanges();
                }
                else
                {
                    currActor = existActor;
                }
                movieData.MovieActors.Add(new MovieActor { Actor = currActor });
            }

            if (!this.data.Movies.Contains(movieData))
            {
                this.data.Movies.Add(movieData);
                this.data.SaveChanges();

                return movieData.MovieId;
            }
            return movieData.MovieId;
        }

        public bool Edit(int id, string[] directorNames, string creatorId,
            string title, int year, string imageUrl, string watchUrl,
            string country, int duration, string descriptions, int genreId, List<string> actors)
        {
            var movieData = this.data.Movies.Find(id);

            if (movieData == null)
            {
                return false;
            }

            var directorForEdit = this.data.Directors
                .Find(movieData.DirectorId);

            if (directorForEdit != null)
            {
                directorForEdit.FirstName = directorNames[0];
                directorForEdit.LastName = directorNames[1];
            }
            else
            {
                var director = new Director
                {
                    FirstName = directorNames[0],
                    LastName = directorNames[1]
                };

                this.data.Directors.Add(director);
                this.data.SaveChanges();

                movieData.DirectorId = director.DirectorId;
            }

            

            movieData.Title = title;
            movieData.Year = year;
            movieData.ImageUrl = imageUrl;
            movieData.WatchUrl = watchUrl;
            movieData.Country = country;
            movieData.Duration = duration;
            movieData.Descriptions = descriptions;
            movieData.GenreId = genreId;

            foreach (var actor in actors)
            {
                var currActor = new Actor
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1],
                    FullName = actor.Split(" ")[0] + " " + actor.Split(" ")[1]
                };

                var actorForEdit = this.data.Actors.FirstOrDefault(a => a.FullName == currActor.FullName);
                if (actorForEdit == null)
                {
                    movieData.MovieActors.Add(new MovieActor { Actor = currActor });
                }
                else
                {
                    actorForEdit.FirstName = currActor.FirstName;
                    actorForEdit.LastName = currActor.LastName;
                    actorForEdit.FullName = currActor.FullName;
                }
            }
            this.data.SaveChanges();

            return true;
        }

        public MovieDetailsServiceModel Details(int id)
        {
            var movieData = this.data.Movies.Where(m => m.MovieId == id).FirstOrDefault();
            var directorFullName = this.data.Directors
                .Where(d => d.DirectorId == movieData.DirectorId)
                .FirstOrDefault();

            var movie = this.data.Movies.Where(m => m.MovieId == id)
                .Select(m => new MovieDetailsServiceModel
                {
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    WatchUrl = m.WatchUrl,
                    Country = m.Country,
                    DirectorId = m.DirectorId,
                    Director = directorFullName.FirstName + " " + directorFullName.LastName,
                    Actors = string.Join(", ", m.MovieActors.Select(a => a.Actor.FullName)), // !!!
                    Duration = m.Duration,
                    Descriptions = m.Descriptions,
                    GenreId = m.GenreId
                }).FirstOrDefault();
            return movie;
        }

        public MovieQueryServiceModel AllMovies(string userId)
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

        public string[] DirectorNames(MovieFormModel movie) => movie.Director.Split(" ");

        public List<string> ActorsList(MovieFormModel movie) => movie.Actors.Split(", ").ToList();

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

        //public bool MovieExists(Movie movie)
        //    => this.data.Movies.Contains(movie);

    }
}
