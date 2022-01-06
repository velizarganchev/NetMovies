namespace NetMovies.Infrastructure
{
    using AutoMapper;
    using Models.Movie;
    using Services.Movies.Models;

    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            this.CreateMap<MovieDetailsServiceModel,MovieFormModel>();
        }
    }
}
