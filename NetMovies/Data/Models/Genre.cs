namespace NetMovies.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Genre
    {
        [Key]
        [Required]
        public string GenreId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
