using NetMovies.Data;
using NetMovies.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NetMovies.Services.Votes
{
    public class VotesService : IVotesService
    {
        private readonly NetMoviesDbContext data;
        public VotesService(NetMoviesDbContext data)
        {
            this.data = data;
        }

        public int GetVotes(int movieId)
        {
            return this.data.Votes.Where(x => x.MovieId == movieId).Sum(x => (int)x.Type);
        }

        public async Task VoteAsync(int movieId, string userId, bool isUpVote)
        {
            var vote = this.data.Votes
                .FirstOrDefault(x => x.MovieId == movieId && x.UserId == userId);
            if (vote != null)
            {
                vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new Vote
                {
                    MovieId = movieId,
                    UserId = userId,
                    Type = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };
                await this.data.Votes.AddAsync(vote);
            }

            await this.data.SaveChangesAsync();
        }
    }
}
