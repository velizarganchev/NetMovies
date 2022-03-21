namespace NetMovies.Models.Movie
{
    using NetMovies.Services.Movies.Models;
    using System;
    using System.Collections.Generic;
    public class AllMovieQueryModel
    {
        public int MoviesPerPage { get; set; } = 4;

        public int CurrentPage { get; set; } = 1;

        public string SearchTerm { get; init; }

        public bool HasPreviosPage => this.CurrentPage > 1;

        public bool HasNextPage => this.CurrentPage < this.PagesCount;

        public int PreviosPageNumber => this.CurrentPage - 1;

        public int NextPageNumber => this.CurrentPage + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.TotalMovies / this.MoviesPerPage);

        public int TotalMovies { get; set; }

        public IEnumerable<MovieGenreServiceModel> Genres { get; set; }

        public IEnumerable<MovieQualityServiceModel> Qualities { get; set; }

        public IEnumerable<MovieServiceModel> Movies { get; set; }
    }
}
