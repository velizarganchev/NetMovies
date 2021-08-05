namespace NetMovies.Models.Home
{
    using System.Collections.Generic;
    public class IndexViewModel
    {
        public int TotalMovies { get; set; }
        public int TotalUsers { get; set; }
        public List<MovieIndexViewModel> Movies{ get; set; }
    }
}
