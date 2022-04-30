namespace NetMovies.Tests.Mocks
{
    using AutoMapper;
    using NetMovies.Infrastructure;

    public static class MapperMock
    {
       public static IMapper Instance
        {
            get
            {
                var mapperConfiguration = new MapperConfiguration(config =>
                {
                    config.AddProfile<MyMappingProfile>();
                });

                return new Mapper(mapperConfiguration);
            }
        }
    }
}
