using System;

namespace NetMovies.Data.Models
{
    public class MovieReview
    {
        public int MovieId { get; init; }

        public virtual Movie Movie { get; init; }

        public int ReviewId { get; init; }

        public virtual Review Review { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }

    }
}
