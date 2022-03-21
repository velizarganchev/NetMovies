namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Director
    {
        [Key]
        [Required]
        public int DirectorId { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string FullName { get; set; }

        public ICollection<Movie> MovieDirectors { get; set; } = new HashSet<Movie>();
    }
}
