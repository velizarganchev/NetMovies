namespace NetMovies.Tests.Mocks
{
    using Microsoft.EntityFrameworkCore;
    using NetMovies.Data;
    using System;
    public static class DatabaseMock
    {
        public static NetMoviesDbContext Instance
        {
            get 
            {
                var dbContextOptions = new DbContextOptionsBuilder<NetMoviesDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new NetMoviesDbContext(dbContextOptions);
            }
        }
    }
}
