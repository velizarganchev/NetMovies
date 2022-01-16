namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    public class Quality
    {
        [Key]
        [Required]
        public int QualityId { get; set; }

        [Required]
        [MaxLength(DefaultNameMaxLenght)]
        public string QualityName { get; set; }

        public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
    }
}
