namespace NetMovies.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity;

    using Data;
    using Data.Models;
    using static WebConstants;
    using System.IO;
    using System.Text.Json;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            MigrateDatabase(serviceProvider);

            SeedGenres(serviceProvider);
            SeedAdministrator(serviceProvider);
            SeedMovies(serviceProvider);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider serviceProvider)
        {
            var data = serviceProvider
                .GetRequiredService<NetMoviesDbContext>();

            data.Database.Migrate();
        }
        private static void SeedGenres(IServiceProvider serviceProvider)
        {
            var data = serviceProvider
                .GetRequiredService<NetMoviesDbContext>();

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

            if (data.Qualities.Any())
            {
                return;
            }
            data.Qualities.AddRange(new[]
            {
                new Quality { QualityName = "HD 1080p" },
                new Quality { QualityName = "720p" },
                new Quality { QualityName = "480p" },
                new Quality { QualityName = "460p" }
            });
            data.SaveChanges();
        }
        private static void SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUsers>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                {
                    return;
                }

                var role = new IdentityRole { Name = AdministratorRoleName };
                await roleManager.CreateAsync(role);

                var user = new AppUsers
                {
                    Email = "admin@abv.bg",
                    UserName = "admin@abv.bg"
                };

                await userManager.CreateAsync(user, "admin1");
                await userManager.AddToRoleAsync(user, role.Name);
            })
                .GetAwaiter()
                .GetResult();

        }
        private static void SeedMovies(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<NetMoviesDbContext>();

            //string movieInJson = "MoviesForecast.json";
            //string jsonString = File.ReadAllText(movieInJson);
            //Movie movieForCast = JsonSerializer.Deserialize<Movie>(jsonString)!;

            //Console.WriteLine(movieForCast);

            if (data.Movies.Any())
            {
                return;
            }

            data.Movies
                .AddRange(new[]
                {
                     new Movie
                    {
                        CreatorId = "65581dff-2a15-42f8-a1ad-702d7500a444",
                        Title = "The Dark Knight",
                        Year = 2008,
                        ImageUrl = "https://m.media-amazon.com/images/I/91KkWf50SoL._SL1500_.jpg",
                        WatchUrl = "https://www.youtube.com/watch?v=_64S_ixM5Ng",
                        AgeLimit = 16,
                        Duration = 152,
                        GenreId = 1,
                        QualityId = 2,
                        Country = new Country { Name = "USA" },
                        MovieDirectors = new[]
                    {
                        new Director
                        {
                            FirstName = "Christopher",
                            LastName = "Nolan",
                            FullName = "Christopher Nolan",
                        }
                    },
                        MovieActors = new[]
                    {
                        new Actor
                        {
                            FirstName = "Christian",
                            LastName = "Bale",
                            FullName = "Christian Bale",
                        },
                        new Actor
                        {
                            FirstName = "Heath",
                            LastName = "Ledger",
                            FullName = "Heath Ledger",
                        },
                        new Actor
                        {
                            FirstName = "Liam",
                            LastName = "Neeson",
                            FullName = "Liam Neeson",
                        },
                        new Actor
                        {
                            FirstName = "Morgan",
                            LastName = "Freeman",
                            FullName = "Morgan Freeman",
                        }

                    },
                        Description = "Gotham City hat viele finstere Gestalten hervorgebracht,doch der Joker ist anders.Mit brillantem Verstand und grotesk geschminktem Gesicht versucht er,Batmans Revier ins Chaos zu stürzen. Dem nihilistischen Clown geht es nicht um Reichtümer oder Macht,er will Anarchie. Und er möchte, dass Gothams Bewohner tatkräftig dabei mithelfen.Deswegen hat er es auf Bezirksstaatsanwalt Harvey Dent abgesehen,einen strahlenden Helden, dessen tadellose Gesinnung der Joker ihm auszutreiben versucht.",
                    },
                     new Movie
                    {
                        CreatorId = "65581dff-2a15-42f8-a1ad-702d7500a444",
                        Title = "Mad Max: Fury Road",
                        Year = 2015,
                        ImageUrl = "https://cache.pressmailing.net/thumbnail/story_hires/458b00c0-5c0f-46fa-90a0-34e8135632fb/image.jpg",
                        WatchUrl = "https://www.youtube.com/watch?v=yBzC7MCl3f8",
                        AgeLimit = 16,
                        Duration = 123,
                        GenreId = 1,
                        QualityId = 2,
                        Country = new Country { Name = "Mexico" },
                        MovieDirectors = new[]
                    {
                        new Director
                        {
                            FirstName = "George",
                            LastName = "Miller",
                            FullName = "George Miller",
                        }
                    },
                        MovieActors = new[]
                    {
                        new Actor
                        {
                            FirstName = "Tom",
                            LastName = "Hardy",
                            FullName = "Tom Hardy",
                        },
                        new Actor
                        {
                            FirstName = "Charlize",
                            LastName = "Theron",
                            FullName = "Charlize Theron",
                        },
                        new Actor
                        {
                            FirstName = "Nicholas",
                            LastName = "Hoult",
                            FullName = "Nicholas Hoult",
                        },
                        new Actor
                        {
                            FirstName = "Mel",
                            LastName = "Gibson",
                            FullName = "Mel Gibson",
                        }

                    },
                        Description = "Die beiden Außenseiter Max und Furiosa treffen in einer kargen Wüstenlandschaft aufeinander, als Furiosa und einige Ex-Sklavinnen auf der Flucht sind. Der Warlord Immortan Joe stellt ihnen nach, denn Frauen sind in der postapokalyptischen Welt kostbare Güter. Schwer bewaffnet und in einem zum Panzer umgebauten Sattelschlepper liefern sich die beiden Parteien einen erbitterten Krieg, der sie im rasanten Tempo durch die Wüste jagt.",
                    },
                     new Movie
                    {
                        CreatorId = "65581dff-2a15-42f8-a1ad-702d7500a444",
                        Title = "Jurassic Park",
                        Year = 2018,
                        ImageUrl = "https://media1.jpc.de/image/w600/front/0/5053083150761.jpg",
                        WatchUrl = "https://www.youtube.com/watch?v=mXBhOAPiaDo",
                        AgeLimit = 16,
                        Duration = 125,
                        GenreId = 1,
                        QualityId = 2,
                        Country = new Country { Name = "Panama" },
                        MovieDirectors = new[]
                    {
                        new Director
                        {
                            FirstName = "Steven",
                            LastName = "Spielberg",
                            FullName = "Steven Spielberg",
                        }
                    },
                        MovieActors = new[]
                    {
                        new Actor
                        {
                            FirstName = "Jeff",
                            LastName = "Goldblum",
                            FullName = "Jeff Goldblum",
                        },
                        new Actor
                        {
                            FirstName = "Sam",
                            LastName = "Neill",
                            FullName = "Sam Neill",
                        },
                        new Actor
                        {
                            FirstName = "Laura",
                            LastName = "Dern",
                            FullName = "Laura Dern",
                        },
                        new Actor
                        {
                            FirstName = "Richard",
                            LastName = "Attenborough",
                            FullName = "Richard Attenborough",
                        }

                    },
                        Description = "Der Milliardär Hammond hat sich einen Traum erfüllt und eine Insel gekauft, auf der Dinosaurier dank Klontechnik zu neuem Leben erweckt werden. Er lädt eine Gruppe von Spezialisten zu einer Testbesichtigung des Jurassic Park ein. Zur Sicherheit ist der von einem großen Elektrozaun umgeben. Doch ein unachtsamer Mitarbeiter legt den Strom lahm und so können die gefährlichen Bestien aus ihren Käfigen ausbrechen. Für die Gäste beginnt ein Überlebenskampf gegen die hungrigen Urzeitmonster",
                    },
                     new Movie
                    {
                        CreatorId = "65581dff-2a15-42f8-a1ad-702d7500a444",
                        Title = "Rocky",
                        Year = 2010,
                        ImageUrl = "https://images-na.ssl-images-amazon.com/images/S/pv-target-images/2596301d69a4d0ce86a41d4ef6fdb5e19eabe66ac45cab63eeb97dc45d840254._RI_V_TTW_.jpg",
                        WatchUrl = "https://www.youtube.com/watch?v=-Hk-LYcavrw",
                        AgeLimit = 12,
                        Duration = 145,
                        GenreId = 1,
                        QualityId = 3,
                        Country = new Country { Name = "United States" },
                        MovieDirectors = new[]
                    {
                        new Director
                        {
                            FirstName = "John",
                            LastName = "Avildsen",
                            FullName = "John Avildsen",
                        }
                    },
                        MovieActors = new[]
                    {
                        new Actor
                        {
                            FirstName = "Sylvester",
                            LastName = "Stallone",
                            FullName = "Sylvester Stallone",
                        },
                        new Actor
                        {
                            FirstName = "Carl",
                            LastName = "Weathers",
                            FullName = "Carl Weathers",
                        },
                        new Actor
                        {
                            FirstName = "Talia",
                            LastName = "Shire",
                            FullName = "Talia Shire",
                        },
                        new Actor
                        {
                            FirstName = "Burt",
                            LastName = "Young",
                            FullName = "Burt Young",
                        }

                    },
                        Description = "Die beiden Außenseiter Max und Furiosa treffen in einer kargen Wüstenlandschaft aufeinander, als Furiosa und einige Ex-Sklavinnen auf der Flucht sind. Der Warlord Immortan Joe stellt ihnen nach, denn Frauen sind in der postapokalyptischen Welt kostbare Güter. Schwer bewaffnet und in einem zum Panzer umgebauten Sattelschlepper liefern sich die beiden Parteien einen erbitterten Krieg, der sie im rasanten Tempo durch die Wüste jagt.",
                    }

                });
            data.SaveChanges();
        }
    }
}