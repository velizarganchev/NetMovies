namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;
    public class Review
    {
        public int ReviewId { get; set; }
        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MovieDescriptionsMaxLenght)]
        public string Description { get; set; }

        public ICollection<MovieReview> MovieReviews { get; set; } = new HashSet<MovieReview>();
        public ICollection<AutorReview> AutorReviews { get; set; } = new HashSet<AutorReview>();
    }
}
