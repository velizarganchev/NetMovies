namespace NetMovies.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    public class AppUsers : IdentityUser
    {
        public ICollection<Movie> Movies { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
