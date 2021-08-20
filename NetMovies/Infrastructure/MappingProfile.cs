namespace NetMovies.Infrastructure
{
    using AutoMapper;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Movie, MovieServiceModel>();
        }
    }
}
