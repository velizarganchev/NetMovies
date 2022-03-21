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

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int Duration { get; set; }

        [Required]
        [MaxLength(MovieDescriptionsMaxLenght)]
        public string Description { get; set; }

        public double? Rate { get; set; }

        public int AgeLimit { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }

        public int QualityId { get; set; }

        public Quality Quality { get; set; }

        public string CreatorId { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Director> MovieDirectors { get; set; } = new HashSet<Director>();

        public ICollection<Actor> MovieActors { get; set; } = new HashSet<Actor>();

    }
}
