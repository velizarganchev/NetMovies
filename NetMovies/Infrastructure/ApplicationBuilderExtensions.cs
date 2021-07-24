namespace NetMovies.Infrastructure
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using NetMovies.Data;
    using NetMovies.Data.Models;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices
                .ServiceProvider
                .GetService<NetMoviesDbContext>();

            data.Database.Migrate();


            return app;
        }

        private static void SeedGenres(NetMoviesDbContext data)
        {
            if (data.Genres.Any())
            {
                return;
            }

            data.Genres.AddRange(new[] 
            {
                new Genre{ Name = "Drama"},
                new Genre{ Name = "Fantasy"},
                new Genre{ Name = "Thriller"},
                new Genre{ Name = "Action"},
                new Genre{ Name = "Horror"},
                new Genre{ Name = "Mystery"},
                new Genre{ Name = "Romance"},
                new Genre{ Name = "Comedy"},
                new Genre{ Name = "Western"},
            });

            data.SaveChanges();
        }
    }
}
