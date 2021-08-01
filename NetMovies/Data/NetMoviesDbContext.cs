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
            .HasMany(c => c.Actors)
                .WithMany(m => m.Movies)
                .UsingEntity<Dictionary<string, object>>(
            "ActorMovie",
            j => j.HasOne<Actor>().WithMany().OnDelete(DeleteBehavior.Restrict),
            j => j.HasOne<Movie>().WithMany().OnDelete(DeleteBehavior.Restrict));

            builder.Entity<Movie>()
                .HasOne(d => d.Director)
                .WithMany(m => m.Movies)
                .HasForeignKey(d => d.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Movie>()
                .HasOne(g => g.Genre)
                .WithMany(m => m.Movies)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
