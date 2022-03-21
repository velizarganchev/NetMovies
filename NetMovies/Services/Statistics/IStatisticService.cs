namespace NetMovies.Services.Statistics
{
    public interface IStatisticService
    {
        StatisticServiceModel Total(); 
        StatisticServiceModel MyTotal(string userId);
    }
}
