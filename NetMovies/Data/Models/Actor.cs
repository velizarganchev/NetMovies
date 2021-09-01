namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Actor
    {
        [Key]
        [Required]
        public int ActorId { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string LastName { get; set; }

        [Required]
        public string FullName { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new HashSet<MovieActor>();
    }
}
