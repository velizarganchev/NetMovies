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
        public int GenreId { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string GenreName { get; set; }

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
