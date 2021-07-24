namespace NetMovies.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Director
    {
        [Key]
        [Required]
        public string DirectorId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string LastName { get; set; }

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
