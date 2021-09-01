
namespace NetMovies.Infrastructure
{
    using AutoMapper;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies.Models;

    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            this.CreateMap<MovieDetailsServiceModel,MovieFormModel>();
        }
    }
}
