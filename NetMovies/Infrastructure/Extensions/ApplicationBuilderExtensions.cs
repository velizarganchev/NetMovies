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
                   new Genre { GenreName = "Action / Adventure" },
                   new Genre { GenreName = "Animals" },
                   new Genre { GenreName = "Animation"},
                   new Genre { GenreName = "Comedy" },
                   new Genre { GenreName = "Cooking" },
                   new Genre { GenreName = "Dance" },
                   new Genre { GenreName = "Documentary" },
                   new Genre { GenreName = "Drama"},
                   new Genre { GenreName = "Education" },
                   new Genre { GenreName = "Entertainment" },
                   new Genre { GenreName = "Family" },
                   new Genre { GenreName = "Fantasy" },
                   new Genre { GenreName = "History" },
                   new Genre { GenreName = "Horror"},
                   new Genre { GenreName = "Independent" },
                   new Genre { GenreName = "International" },
                   new Genre { GenreName = "Kids"},
                   new Genre { GenreName = "Kids & Family" },
                   new Genre { GenreName = "Medical" },
                   new Genre { GenreName = "Military / War" },
                   new Genre { GenreName = "Music" },
                   new Genre { GenreName = "Musical" },
                   new Genre { GenreName = "Mystery / Crime"},
                   new Genre { GenreName = "Nature" },
                   new Genre { GenreName = "Paranormal" },
                   new Genre { GenreName = "Politics" },
                   new Genre { GenreName = "Racing" },
                   new Genre { GenreName = "Romance" },
                   new Genre { GenreName = "Sci - Fi / Horror"},
                   new Genre { GenreName = "Science"},
                   new Genre { GenreName = "Science Fiction" },
                   new Genre { GenreName = "Science / Nature" },
                   new Genre { GenreName = "Spanish" },
                   new Genre { GenreName = "Travel" },
                   new Genre { GenreName = "Western" },
            });
            data.Qualities.AddRange(new[]
            {
                new Quality { QualityName = "HD 1080p" },
                new Quality { QualityName = "720p" },
                new Quality { QualityName = "480p" },
                new Quality { QualityName = "460p" }
            });
            data.SaveChanges();
        }
    }
}
