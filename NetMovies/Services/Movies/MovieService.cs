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
            .Where(m => m.IsDeleted == false)
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
                .Where(m => m.IsDeleted == false)
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
                .Where(m => m.IsDeleted == false)
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

        public int Create(List<string> directors, string creatorId,
            string title, int year, string imageUrl, string watchUrl,
            string country, int duration, string descriptions, int genreId, List<string> actors)
        {
            directors.RemoveAt(directors.Count - 1);

            foreach (var directorNames in directors)
            {
                var director = new Director
                {
                    FirstName = directorNames.Split(" ")[0],
                    LastName = directorNames.Split(" ")[1],
                    FullName = directorNames.Split(" ")[0] + " " + directorNames.Split(" ")[1]
                };
                var existDirector = this.data
                    .Directors
                    .FirstOrDefault(x => x.FullName == director.FullName);

                if (!this.data.Directors.Contains(existDirector))
                {
                    this.data.Directors.Add(director);
                    this.data.SaveChanges();
                }
                else
                {
                    director.DirectorId = existDirector.DirectorId;
                }
            }

            var movieData = new Movie
            {
                CreatorId = creatorId,
                Title = title,
                Year = year,
                ImageUrl = imageUrl,
                WatchUrl = watchUrl,
                Country = country,
                Duration = duration,
                Descriptions = descriptions,
                GenreId = genreId
            };

            actors.RemoveAt(actors.Count - 1);

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

        public bool Edit(int id, List<string> directors, string creatorId,
            string title, int year, string imageUrl, string watchUrl,
            string country, int duration, string descriptions, int genreId, List<string> actors)
        {
            var movieData = this.data.Movies.Find(id);

            if (movieData == null)
            {
                return false;
            }

            foreach (var directorNames in directors)
            {
                var currDirector = new Director
                {
                    FirstName = directorNames.Split(" ")[0],
                    LastName = directorNames.Split(" ")[1],
                    FullName = directorNames.Split(" ")[0] + " " + directorNames.Split(" ")[1]
                };


                var directorForEdit = this.data.Directors
                    .FirstOrDefault(d => d.FullName == currDirector.FullName);

                if (directorForEdit == null)
                {
                    movieData.MovieDirectors.Add(new MovieDirector { Director = currDirector });
                }
                else
                {
                    directorForEdit.FirstName = currDirector.FirstName;
                    directorForEdit.LastName = currDirector.LastName;
                    directorForEdit.FullName = currDirector.FullName;
                }
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

            var movie = this.data.Movies.Where(m => m.MovieId == id)
                .Where(m => m.IsDeleted == false)
                .Select(m => new MovieDetailsServiceModel
                {
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    WatchUrl = m.WatchUrl,
                    Country = m.Country,
                    DirectorId = int.Parse(string.Join(",", m.MovieDirectors.Select(md => md.Director.DirectorId))),
                    Directors = string.Join(",", m.MovieDirectors.Select(md => md.Director.FullName)),
                    Actors = string.Join(", ", m.MovieActors.Select(ma => ma.Actor.FullName)),
                    Duration = m.Duration,
                    Descriptions = m.Descriptions,
                    GenreId = m.GenreId,
                    CreatorId = m.CreatorId
                }).FirstOrDefault();
            return movie;
        }

        public MovieQueryServiceModel AllMovies(string userId)
        {
            var moviesQuery = this.data.Movies.Where(m => m.CreatorId == userId).AsQueryable();

            var movies = moviesQuery
                .OrderByDescending(m => m.MovieId)
                .Where(m => m.IsDeleted == false)
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

        public List<string> DirectorsList(MovieFormModel movie) => movie.Directors.Split(", ").ToList();

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

        public bool Delete(int id)
        {
            var movie = this.data.Movies.FirstOrDefault(m => m.MovieId == id);
            if (movie != null)
            {
                movie.IsDeleted = true;
                this.data.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
