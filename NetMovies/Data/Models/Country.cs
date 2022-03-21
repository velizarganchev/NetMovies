namespace NetMovies.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}
