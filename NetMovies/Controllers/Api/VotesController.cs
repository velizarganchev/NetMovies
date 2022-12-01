using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NetMovies.Models.Votes;
using NetMovies.Services.Votes;
using NetMovies.Infrastructure.Extensions;
using Microsoft.AspNetCore.Cors;

namespace NetMovies.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        [Authorize]
        [EnableCors("nameOfSite")]
        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Votes(VoteInputModel input)
        {
            await this.votesService.VoteAsync(input.MovieId, this.User.Id(), input.isUpVote);
            var votes = this.votesService.GetVotes(input.MovieId);

            return new VoteResponseModel { VotesCount = votes, };
        }
    }
}
