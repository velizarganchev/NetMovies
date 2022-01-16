namespace NetMovies.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class NetMoviesDbContext : IdentityDbContext
    {
        public NetMoviesDbContext(DbContextOptions<NetMoviesDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; init; }
        public DbSet<Actor> Actors { get; init; }
        public DbSet<MovieActor> MovieActors { get; init; }
        public DbSet<Autor> Autors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AutorReview> AutorReviews { get; set; }
        public DbSet<MovieReview> MovieReviews { get; set; }
        public DbSet<Director> Directors { get; init; }
        public DbSet<MovieDirector> MovieDirectors { get; init; }
        public DbSet<Genre> Genres { get; init; }
        public DbSet<Quality> Qualities { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MovieActor>()
                .HasKey(k => new { k.ActorId, k.MovieId });

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

            builder.Entity<AutorReview>()
                .HasKey(k => new { k.ReviewId, k.AutorId});

            builder.Entity<AutorReview>()
                .HasOne(ra => ra.Review)
                .WithMany(r => r.AutorReviews)
                .HasForeignKey(ra => ra.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AutorReview>()
                .HasOne(ra => ra.Autor)
                .WithMany(a => a.AutorReviews)
                .HasForeignKey(ra => ra.AutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MovieDirector>()
                .HasKey(k => new { k.MovieId, k.DirectorId});

            builder.Entity<MovieDirector>()
                .HasOne(md => md.Movie)
                .WithMany(m => m.MovieDirectors)
                .HasForeignKey(md => md.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MovieDirector>()
                .HasOne(md => md.Director)
                .WithMany(d => d.MovieDirectors)
                .HasForeignKey(md => md.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MovieReview>()
                .HasKey(k => new { k.MovieId, k.ReviewId});

            builder.Entity<MovieReview>()
                .HasOne(mr => mr.Movie)
                .WithMany(m => m.MovieReviews)
                .HasForeignKey(mr => mr.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MovieReview>()
                .HasOne(mr => mr.Review)
                .WithMany(r => r.MovieReviews)
                .HasForeignKey(mr => mr.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);

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
