namespace NetMovies.Infrastructure
{
    using AutoMapper;
    using NetMovies.Data.Models;
    using NetMovies.Services.Movies.Models;
    using System.Linq;

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

            this.CreateMap<Movie, MovieDetailsServiceModel>()
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(x => string.Join(", ", x.MovieActors.Select(md => md.FullName))))
                .ForMember(dest => dest.Directors, opt => opt.MapFrom(x => string.Join(", ", x.MovieDirectors.Select(md => md.FullName))))
                .ForMember(dest => dest.VotesCount, opt => opt.MapFrom(x => x.Votes.Sum(v => (int)v.Type))); 

            this.CreateMap<Genre, MovieGenreServiceModel>();                          
        }

    }
}
