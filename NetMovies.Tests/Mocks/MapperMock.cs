namespace NetMovies.Tests.Mocks
{
    using AutoMapper;
    using Moq;

    public static class MapperMock
    {
       public static IMapper Instance
        {
            get
            {
                var mapperMock = new Mock<IMapper>();
                mapperMock
                    .SetupGet(x => x.ConfigurationProvider)
                    .Returns(Mock.Of<IConfigurationProvider>());

                return mapperMock.Object;
            }
        }
    }
}
