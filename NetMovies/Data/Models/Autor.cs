namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Autor
    {
        [Key]
        [Required]
        public int AutorId { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<AutorReview> AutorReviews { get; set; } = new HashSet<AutorReview>();
    }
}
