namespace NetMovies.Services.Movies
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using NetMovies.Data;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies.Models;
    using NetMovies.Services.Statistics;

    public class MovieService : IMovieService
    {
        private readonly NetMoviesDbContext data;
        private readonly IStatisticService statistics;
        private readonly IConfigurationProvider mapper;
        public MovieService(
            NetMoviesDbContext data,
            IStatisticService statistics, IMapper mapper)
        {
            this.data = data;
            this.statistics = statistics;
            this.mapper = mapper.ConfigurationProvider;
        }



        public MovieQueryServiceModel Index()
        {
            var movies = this.data
            .Movies
            .OrderByDescending(m => m.MovieId)
            .Where(m => m.IsDeleted == false)
            .ProjectTo<MovieServiceModel>(this.mapper)
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
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();

            var totalStatistics = this.statistics.Total();
            var genres = GenreCategories();
            var qualities = Qualities();

            return new MovieQueryServiceModel
            {
                Genres = genres,
                Qualities = qualities,
                TotalMovies = totalStatistics.TotalMovies,
                Movies = movies
            };
        }

        public IEnumerable<MovieServiceModel> AllApiMovies()
        {
            var movies = this.data.Movies.OrderBy(m => m.MovieId)
                .Where(m => m.IsDeleted == false)
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();

            return movies;
        }

        public IEnumerable<MovieServiceModel> MyApiMovies(string userId)
        {
            var movies = this.data.Movies.Where(m => m.CreatorId == userId)
                .OrderByDescending(m => m.MovieId)
                .Where(m => m.IsDeleted == false)
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();

            return movies;
        }
        public int Create(List<string> directors, string creatorId, MovieFormModel movie, List<string> actors)
        {
            var movieData = new Movie
            {
                CreatorId = creatorId,
                Title = movie.Title,
                Year = movie.Year,
                ImageUrl = movie.ImageUrl,
                WatchUrl = movie.WatchUrl,
                Country = movie.Country,
                Duration = movie.Duration,
                AgeLimit = movie.AgeLimit,
                Description = movie.Descriptions,
                GenreId = movie.GenreId,
                QualityId = movie.QualityId,
                Rate = 0.0
            };

            foreach (var director in directors)
            {
                var currdirector = new Director
                {
                    FirstName = director.Split(" ")[0],
                    LastName = director.Split(" ")[1],
                    FullName = director.Split(" ")[0] + " " + director.Split(" ")[1]
                };
                var existDirector = this.data.Directors.FirstOrDefault(x => x.FullName == currdirector.FullName);

                if (!this.data.Directors.Contains(existDirector))
                {
                    movieData.MovieDirectors.Add(new MovieDirector { Director = currdirector });
                }
                else
                {
                    movieData.MovieDirectors.Add(new MovieDirector { Director = existDirector });
                }
            }

            foreach (var actor in actors)
            {
                var currActor = new Actor
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1],
                    FullName = actor.Split(" ")[0] + " " + actor.Split(" ")[1]
                };

                var existActor = this.data.Actors.FirstOrDefault(x => x.FullName == currActor.FullName);

                if (!this.data.Actors.Contains(existActor))
                {
                    movieData.MovieActors.Add(new MovieActor { Actor = currActor });
                }
                else
                {
                    movieData.MovieActors.Add(new MovieActor { Actor = currActor });
                }
            }

            var existMovie = this.data.Movies.FirstOrDefault(x => x.Title == movieData.Title);

            if (existMovie == null)
            {
                this.data.Movies.Add(movieData);
                this.data.SaveChanges();

                return movieData.MovieId;
            }
            return existMovie.MovieId;
        }

        public bool Edit(int id, List<string> directors, string creatorId, MovieFormModel movie, List<string> actors)
        {
            var movieData = this.data.Movies.FirstOrDefault(x => x.MovieId == id);

            var movieDirectors = this.data.Movies.Where(m => m.MovieId == id)
                .Select(m => new
                {
                    Directors = m.MovieDirectors.Select(d => new
                    {
                        FirstName = d.Director.FirstName,
                        LastName = d.Director.LastName,
                        FullName = d.Director.FullName,
                    })
                }).FirstOrDefault();

            movieData.MovieDirectors.Remove(movieData.MovieDirectors.FirstOrDefault());

            // изтрии всички акт и всички дир на филма и тогава проверявай дали акт от импута го  има в базата ако го има добавяй 
            // съществуващия иначе добавяй подадения.. Как да изтрия дир и акт от филма но да не ги трия от базата ???????

            var movieDirectorsAndActors = this.data.Movies.Where(m => m.MovieId == id)
                .Select(m => new
                {
                    Directors = m.MovieDirectors.Select(md => md.Director.FullName),
                    Actors = m.MovieActors.Select(ma => ma.Actor.FullName)
                })
                .FirstOrDefault();

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
                    FullName = directorNames.Split(" ")[0] + " " + directorNames.Split(" ")[1],
                };

                var existDirector = this.data.Directors.FirstOrDefault(x => x.FullName == currDirector.FullName);

                if (existDirector != null)
                {
                    if (!movieDirectorsAndActors.Directors.Contains(currDirector.FullName))
                    {
                        movieData.MovieDirectors.Add(new MovieDirector { Director = existDirector });
                    }
                }
                else
                {
                    movieData.MovieDirectors.Add(new MovieDirector { Director = currDirector });
                }
            }

            movieData.Title = movie.Title;
            movieData.Year = movie.Year;
            movieData.ImageUrl = movie.ImageUrl;
            movieData.WatchUrl = movie.WatchUrl;
            movieData.Country = movie.Country;
            movieData.Duration = movie.Duration;
            movieData.AgeLimit = movie.AgeLimit;
            movieData.Description = movie.Descriptions;
            movieData.GenreId = movie.GenreId;
            movieData.QualityId = movie.QualityId;
            movieData.Rate = 0.0;

            foreach (var actor in actors)
            {
                var currActor = new Actor
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1],
                    FullName = actor.Split(" ")[0] + " " + actor.Split(" ")[1],
                };

                var existActor = this.data.Actors.FirstOrDefault(x => x.FullName == currActor.FullName);

                if (existActor != null)
                {
                    if (!movieDirectorsAndActors.Actors.Contains(currActor.FullName))
                    {
                        movieData.MovieActors.Add(new MovieActor { Actor = existActor });
                    }
                }
                else
                {
                    movieData.MovieActors.Add(new MovieActor { Actor = currActor });
                }
            }
            this.data.SaveChanges();
            return true;
        }

        public MovieDetailsServiceModel Details(int id)
        {
            var movie = this.data.Movies.Where(m => m.MovieId == id)
                .Where(m => m.IsDeleted == false)
                .Select(m => new MovieDetailsServiceModel
                {
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    WatchUrl = m.WatchUrl,
                    AgeLimit = m.AgeLimit,
                    Country = m.Country,
                    Directors = string.Join(", ", m.MovieDirectors.Select(md => md.Director.FullName)),
                    Actors = string.Join(", ", m.MovieActors.Select(ma => ma.Actor.FullName)),
                    Duration = m.Duration,
                    Description = m.Description,
                    Genre = m.Genre.GenreName,
                    GenreId = m.GenreId,
                    Quality = m.Quality.QualityName,
                    QualityId = m.QualityId,
                    CreatorId = m.CreatorId
                }).FirstOrDefault();

            return movie;
        }

        public MovieQueryServiceModel MyMovies(string userId)
        {
            var moviesQuery = this.data.Movies.Where(m => m.CreatorId == userId).AsQueryable();

            var movies = moviesQuery
                .OrderByDescending(m => m.MovieId)
                .Where(m => m.IsDeleted == false)
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();

            return new MovieQueryServiceModel
            {
                Movies = movies,
                TotalMovies = movies.Count
            };
        }

        public List<string> DirectorsList(MovieFormModel movie) => movie.Directors.Split(", ").ToList();

        public List<string> ActorsList(MovieFormModel movie) => movie.Actors.Split(", ").ToList();

        public IEnumerable<MovieGenreServiceModel> GenreCategories()
        => data.Genres.ProjectTo<MovieGenreServiceModel>(this.mapper).ToList();
        public IEnumerable<MovieQualityServiceModel> Qualities()
        =>
            data.Qualities.Select(q => new MovieQualityServiceModel
            {
                QualityId = q.QualityId,
                Name = q.QualityName
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
