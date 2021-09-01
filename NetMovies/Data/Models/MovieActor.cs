namespace NetMovies.Data.Models
{
    public class MovieActor
    {
        public int MovieId { get; init; }
        public virtual Movie Movie { get; init; }
        public int ActorId  { get; init; }
        public Actor Actor { get; init; }
    }
}
