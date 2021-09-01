namespace NetMovies.Data
{
    using NetMovies.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class NetMoviesDbContext : IdentityDbContext
    {
        public NetMoviesDbContext(DbContextOptions<NetMoviesDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<Actor> Actors { get; init; }
        public DbSet<MovieActor> MovieActors { get; init; }
        public DbSet<Director> Directors { get; init; }
        public DbSet<Genre> Genres { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MovieActor>()
                .HasKey(k => new { k.ActorId, k.MovieId});

            builder.Entity<MovieActor>()
                .HasOne(mc => mc.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(mc => mc.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MovieActor>()
                .HasOne(mc => mc.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(mc => mc.ActorId)
                .OnDelete(DeleteBehavior.Restrict);

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
