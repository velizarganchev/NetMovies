namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Movie
    {
        [Key]
        [Required]
        public int MovieId { get; set; }

        [Required]
        [MaxLength(MovieTitleMaxLenght)]
        public string Title { get; set; }

        public int Year { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string WatchUrl { get; set; }

        [Required]
        [MaxLength(MovieCountryMaxLenght)]
        public string Country { get; set; }

        public int Duration { get; set; }

        [Required]
        [MaxLength(MovieDescriptionsMaxLenght)]
        public string Descriptions { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();

        public ICollection<MovieDirector> MovieDirectors { get; set; } = new HashSet<MovieDirector>();

        public ICollection<MovieReview> MovieReviews { get; set; } = new HashSet<MovieReview>();
    }
}
