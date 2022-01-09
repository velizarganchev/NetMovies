namespace NetMovies.Infrastructure
{
    using System.Linq;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Data;
    using Data.Models;
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

            SeedGenres(data);

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
                new Genre{ GenreName = "Drama"},
                new Genre{ GenreName = "Fantasy"},
                new Genre{ GenreName = "Thriller"},
                new Genre{ GenreName = "Action"}, 
                new Genre{ GenreName = "Horror"},
                new Genre{ GenreName = "Mystery"},
                new Genre{ GenreName = "Romance"},
                new Genre{ GenreName = "Comedy"},
                new Genre{ GenreName = "Western"},
                new Genre{ GenreName = "Animation"}
            });

            data.SaveChanges();
        }
    }
}
