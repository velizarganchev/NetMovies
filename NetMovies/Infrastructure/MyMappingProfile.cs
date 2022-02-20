namespace NetMovies.Infrastructure
{
    using AutoMapper;
    using NetMovies.Data.Models;
    using NetMovies.Services.Movies.Models;

    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            this.CreateMap<Movie, MovieServiceModel>()
                .ForMember(dest =>
                dest.Genre,
                opt => opt.MapFrom(x => x.Genre.GenreName))
                .ForMember(des =>
                des.Quality,
                opt => opt.MapFrom(x => x.Quality.QualityName));

            this.CreateMap<Movie, MovieDetailsServiceModel>();

            this.CreateMap<Genre, MovieGenreServiceModel>();

            //.Select(m => new MovieDetailsServiceModel
            // {
            //     Title = m.Title,
            //     Year = m.Year,
            //     ImageUrl = m.ImageUrl,
            //     WatchUrl = m.WatchUrl,
            //     AgeLimit = m.AgeLimit,
            //     Country = m.Country,
            //     Directors = string.Join(", ", m.MovieDirectors.Select(md => md.Director.FullName)),
            //     Actors = string.Join(", ", m.MovieActors.Select(ma => ma.Actor.FullName)),
            //     Duration = m.Duration,
            //     Description = m.Description,
            //     GenreId = m.GenreId,
            //     Quality = m.Quality.QualityName,
            //     CreatorId = m.CreatorId
            // })
        }

    }
}
