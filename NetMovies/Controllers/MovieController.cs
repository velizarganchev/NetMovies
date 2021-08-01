﻿namespace NetMovies.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using NetMovies.Data;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;

    public class MovieController : Controller
    {
        private readonly NetMoviesDbContext data;

        public MovieController(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public IActionResult Add() => View(new AddMovieFormModel
        {
            Genres = this.GetGenreCategories()
        });

        [HttpPost]
        public IActionResult Add(AddMovieFormModel movie)
        {
            if (!this.data.Genres.Any(g => g.GenreId == movie.GenreId))
            {
                this.ModelState.AddModelError(nameof(movie.GenreId), "Genre does not exist.");
            }
            if (!ModelState.IsValid)
            {
                movie.Genres = this.GetGenreCategories();

                return View(movie);
            }

            var director = new Director 
            {
                FirstName = movie.Director.Split(" ")[0],
                LastName = movie.Director.Split(" ")[1] 
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
                Title = movie.Title,
                DirectorId = director.DirectorId,
                Year = movie.Year,
                ImageUrl = movie.ImageUrl,
                Country = movie.Country,
                Duration = movie.Duration,
                Descriptions = movie.Descriptions,
                GenreId = movie.GenreId
            };

            var actorsList =  movie.Actors.Split(", ").ToList();
            foreach (var actor in actorsList)
            {
                var currActor = new Actor 
                {
                    FirstName = actor.Split(" ")[0],
                    LastName = actor.Split(" ")[1]
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

            if (this.data.Movies.Contains(movieData))
            {
                return RedirectToAction("Add", "Movie");
            }

            this.data.Movies.Add(movieData);
            this.data.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<MovieGenreViewModel> GetGenreCategories()
            => data.Genres.Select(g => new MovieGenreViewModel
            {
                GenreId = g.GenreId,
                Name = g.GenreName
            })
            .ToList();
    }
}
