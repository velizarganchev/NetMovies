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
    using Microsoft.EntityFrameworkCore;

    public class MovieService : IMovieService
    {
        private readonly NetMoviesDbContext data;
        private readonly IStatisticService statistics;
        private readonly IConfigurationProvider mapper;
        public MovieService(
            NetMoviesDbContext data,
            IStatisticService statistics,
            IMapper mapper)
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
                m.MovieActors.FirstOrDefault(a => a.FullName == searchTerm).FullName == searchTerm);
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
                Movies = movies,
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
                Duration = movie.Duration,
                AgeLimit = movie.AgeLimit,
                Description = movie.Descriptions,
                GenreId = movie.GenreId,
                QualityId = movie.QualityId,
            };

            var existCountry = data.Countries.FirstOrDefault(x => x.Name == movie.Country);

            if (existCountry == null)
            {
                movieData.Country = new Country { Name = movie.Country };
            }
            else
            {
                movieData.Country = existCountry;
            }

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
                    movieData.MovieDirectors.Add(currdirector);
                }
                else
                {
                    movieData.MovieDirectors.Add(existDirector);
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
                    movieData.MovieActors.Add(currActor);
                }
                else
                {
                    movieData.MovieActors.Add(existActor);
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
            var movieData = this.data.Movies
                .Where(x => x.MovieId == id )
                .Include(x => x.MovieDirectors)
                .Include(x => x.MovieActors)
                .FirstOrDefault();

            movieData.MovieDirectors.Clear();
            movieData.MovieActors.Clear();

            if (movieData == null)
            {
                return false;
            }

            movieData.Title = movie.Title;
            movieData.Year = movie.Year;
            movieData.ImageUrl = movie.ImageUrl;
            movieData.WatchUrl = movie.WatchUrl;
            movieData.Duration = movie.Duration;
            movieData.AgeLimit = movie.AgeLimit;
            movieData.Description = movie.Descriptions;
            movieData.GenreId = movie.GenreId;
            movieData.QualityId = movie.QualityId;

            var existCountry = data.Countries.FirstOrDefault(x => x.Name == movie.Country);

            if (existCountry == null)
            {
                movieData.Country = new Country { Name = movie.Country };
            }
            else
            {
                movieData.Country = existCountry;
            }

            foreach (var directorNames in directors)
            {
                var currDirector = new Director
                {
                    FirstName = directorNames.Split(" ")[0],
                    LastName = directorNames.Split(" ")[1],
                    FullName = directorNames.Split(" ")[0] + " " + directorNames.Split(" ")[1],
                };

                var existDirectorInData = this.data.Directors.FirstOrDefault(x => x.FullName == currDirector.FullName);

                if (existDirectorInData != null)
                {
                    movieData.MovieDirectors.Add(existDirectorInData);
                }
                else
                {
                    movieData.MovieDirectors.Add(currDirector);
                }
            }

            foreach (var actor in actors)
            {
                var currActor = new Actor
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1],
                    FullName = actor.Split(" ")[0] + " " + actor.Split(" ")[1],
                };

                var existActorInData = this.data.Actors.FirstOrDefault(x => x.FullName == currActor.FullName);

                if (existActorInData != null)
                {
                    movieData.MovieActors.Add(existActorInData);
                }
                else
                {
                    movieData.MovieActors.Add(currActor);
                }
            }

            this.data.SaveChanges();
            return true;
        }

        public MovieDetailsServiceModel Details(int id, string userId)
        {
            var movie = this.data.Movies.Where(m => m.MovieId == id)
                .Where(m => m.IsDeleted == false)
                .Select(m => new MovieDetailsServiceModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Year = m.Year,
                    ImageUrl = m.ImageUrl,
                    WatchUrl = m.WatchUrl,
                    AgeLimit = m.AgeLimit,
                    Country = m.Country.Name,
                    Directors = string.Join(", ", m.MovieDirectors.Select(md => md.FullName)),
                    Actors = string.Join(", ", m.MovieActors.Select(ma => ma.FullName)),
                    Duration = m.Duration,
                    Description = m.Description,
                    Genre = m.Genre.GenreName,
                    GenreId = m.GenreId,
                    Quality = m.Quality.QualityName,
                    QualityId = m.QualityId,
                    CreatorId = m.CreatorId,
                    VotesCount = m.Votes.Sum(v => (int)v.Type),
                    isAddedInMyList = m.Users.Any(x => x.Id == userId),

                }).FirstOrDefault();

            return movie;
        }


        public MovieQueryServiceModel MyMovies(
            string userId,
            bool isAdmin,
            int currentPage,
            int moviesPerPage)
        {
            var movies = this.data.Movies.Where(m => m.CreatorId == userId || isAdmin == true)
                .Skip((currentPage - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .OrderByDescending(m => m.MovieId)
                .Where(m => m.IsDeleted == false)
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();

            var totalStatistics = this.statistics.MyTotal(userId, isAdmin);

            return new MovieQueryServiceModel
            {
                Movies = movies,
                TotalMovies = totalStatistics.MyTotalMovies,
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

        public bool Remove(int movieId, string userId)
        {
            var movie = this.data.Movies.Include(x => x.Users).FirstOrDefault(m => m.MovieId == movieId);         

            if (movie != null)
            {
                var user = movie.Users.FirstOrDefault(x => x.Id == userId);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    movie.Users.Remove(user);
                    this.data.SaveChanges();
                    return true;
                }              
            }
            return false;
        }

        public string AddUserToMovie(int movieId, AppUsers user)
        {

            var movie = this.data.Movies
                .Where(x => x.MovieId == movieId)
                .Include(x => x.Users)
                .FirstOrDefault();

            if (movie.Users.Contains(user))
            {
                return user.Id;
            }
            else
            {
                movie.Users.Add(user);

                this.data.SaveChanges();
                return user.Id;
            }          
        }

        public MovieQueryServiceModel MyListMovies(string userId, int currentPage, int moviesPerPage)
        {
            var movies = this.data.Movies
                .Where(x => x.Users.Any(x => x.Id == userId))
                .Skip((currentPage - 1) * moviesPerPage)
                .Take(moviesPerPage)
                .OrderByDescending(m => m.MovieId)
                .Where(m => m.IsDeleted == false)
                .ProjectTo<MovieServiceModel>(this.mapper)
                .ToList();

            return new MovieQueryServiceModel
            {
                TotalMovies = movies.Count(),
                Movies = movies,
            };
        }
    }
}
