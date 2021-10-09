namespace NetMovies.Data.Models
{
    public class AutorReview
    {
        public int AutorId { get; set; }
        public virtual Autor Autor { get; set; }
        public int ReviewId { get; init; }
        public virtual Review Review { get; set; }

    }
}
