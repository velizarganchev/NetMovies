using System.Threading.Tasks;

namespace NetMovies.Services.Votes
{
   public interface IVotesService
    {
        Task VoteAsync(int movieId, string userId, bool isUpVote);

        int GetVotes(int movieId);
    }
}
