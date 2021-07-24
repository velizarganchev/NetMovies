namespace NetMovies.Data
{
    using NetMovies.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class NetMoviesDbContext : IdentityDbContext
    {
        public NetMoviesDbContext(DbContextOptions<NetMoviesDbContext> options)
            : base(options)
        {
        }
        

        public DbSet<Movie> Movies { get; init; }
        public DbSet<Actor> Actors { get; init; }
        public DbSet<Director> Directors { get; init; }
        public DbSet<Genre> Genres { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movie>()
                .HasMany(a => a.Actors)
                .WithMany(m => m.Movies)
                .UsingEntity<Dictionary<string, object>>(
            "ActorMovie",
            j => j.HasOne<Actor>().WithMany().OnDelete(DeleteBehavior.Restrict),
            j => j.HasOne<Movie>().WithMany().OnDelete(DeleteBehavior.Restrict));

            builder.Entity<Movie>()
                .HasMany(d => d.Directors)
                .WithMany(m => m.Movies)
                .UsingEntity<Dictionary<string, object>>(
            "DirectorMovie",
            j => j.HasOne<Director>().WithMany().OnDelete(DeleteBehavior.Restrict),
            j => j.HasOne<Movie>().WithMany().OnDelete(DeleteBehavior.Restrict));

            builder.Entity<Movie>()
                .HasMany(g => g.Genres)
                .WithMany(m => m.Movies)
                .UsingEntity<Dictionary<string, object>>(
            "GenreMovie",
            j => j.HasOne<Genre>().WithMany().OnDelete(DeleteBehavior.Restrict),
            j => j.HasOne<Movie>().WithMany().OnDelete(DeleteBehavior.Restrict));

            base.OnModelCreating(builder);
        }
    }
}
