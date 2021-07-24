namespace NetMovies.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    public class Movie
    {
        [Key]
        [Required]
        public string MovieId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(MovieNameMaxLenght)]
        public string Name { get; set; }

        public int Year { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(MovieCountryMaxLenght)]
        public string Country { get; set; }

        public int Duration { get; set; }

        [Required]
        [MaxLength(MovieDescriptionsMaxLenght)]
        public string Descriptions { get; set; }

        public ICollection<Actor> Actors { get; set; } = new HashSet<Actor>();

        public ICollection<Director> Directors { get; set; } = new HashSet<Director>();

        public ICollection<Genre> Genres { get; set; } = new HashSet<Genre>();
    }
}
