namespace NetMovies.Services.Statistics
{
    using NetMovies.Data;
    using System.Linq;

    public class StatisticService : IStatisticService
    {
        private readonly NetMoviesDbContext data;

        public StatisticService(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public StatisticServiceModel Total()
        {
            var totalMovies = this.data.Movies.Count();


            return new StatisticServiceModel
            {
                TotalMovies = totalMovies,
            };
        }
    }
}
