namespace NetMovies.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class NetMoviesDbContext : IdentityDbContext<AppUsers>
    {
        public NetMoviesDbContext(DbContextOptions<NetMoviesDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<Actor> Actors { get; init; }
        public DbSet<Director> Directors { get; init; }
        public DbSet<Country> Countries { get; init; }
        public DbSet<Genre> Genres { get; init; }
        public DbSet<Quality> Qualities { get; init; }
        public DbSet<Vote> Votes { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Movie>()
                .HasOne(g => g.Genre)
                .WithMany(m => m.Movies)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Movie>()
                .HasOne(q => q.Quality)
                .WithMany(m => m.Movies)
                .HasForeignKey(q => q.QualityId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
