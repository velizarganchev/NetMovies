namespace NetMovies.Data.Models
{
    public class MovieDirector
    {
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
    }
}
