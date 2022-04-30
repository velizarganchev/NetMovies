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
        public StatisticServiceModel MyTotal(string userId, bool isAdmin)
        {
            var myTotalMovies = this.data.Movies.Where(m => m.CreatorId == userId || isAdmin == true).Count();


            return new StatisticServiceModel
            {
                MyTotalMovies = myTotalMovies,
            };
        }
    }
}
