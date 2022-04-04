using System.ComponentModel.DataAnnotations;

namespace NetMovies.Data.Models
{
    public class Vote
    {
        public int VoteId { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        [Required]
        public string UserId { get; set; }

        public AppUsers User { get; set; }

        public VoteType Type { get; set; }
    }
}
