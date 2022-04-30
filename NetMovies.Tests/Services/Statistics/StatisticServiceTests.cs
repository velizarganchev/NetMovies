namespace NetMovies.Tests.Services.Statistics
{
    using NetMovies.Data.Models;
    using NetMovies.Services.Statistics;
    using NetMovies.Tests.Mocks;
    using Xunit;

    public class StatisticServiceTests
    {
        [Fact]
        public void ShouldReturnCorrectMoviesTotalCount()
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            data.Movies.AddRange(new[] { new Movie { MovieId = 111 }, new Movie { MovieId = 112 } });
            data.SaveChanges();

            var statisticService = new StatisticService(data);

            //Act

            var result = statisticService.Total();

            //Assert

            Assert.Equal("2", result.TotalMovies.ToString());

        }

        [Fact]
        public void ShouldReturnCorrectMyMoviesTotalCount()
        {
            const string creatorId = "Veli";
            //Arrange
            using var data = DatabaseMock.Instance;

            data.Movies.AddRange(new[]
            {
                new Movie { MovieId = 111,CreatorId = creatorId, },
                new Movie { MovieId = 112, CreatorId = creatorId }
            });
            data.SaveChanges();

            var statisticService = new StatisticService(data);

            //Act

            var result = statisticService.MyTotal(creatorId,false);

            //Assert

            Assert.Equal("2", result.MyTotalMovies.ToString());
        }
        [Fact]
        public void ShouldReturnCorrectMyMoviesTotalCountWhenIsAdminIsTrue()
        {
            const string creatorId = "Veli";
            //Arrange
            using var data = DatabaseMock.Instance;

            data.Movies.AddRange(new[]
            {
                new Movie { MovieId = 111,CreatorId = creatorId, },
                new Movie { MovieId = 112, CreatorId = creatorId }
            });
            data.SaveChanges();

            var statisticService = new StatisticService(data);

            //Act

            var result = statisticService.MyTotal("Marin", true);

            //Assert

            Assert.Equal("2", result.MyTotalMovies.ToString());
        }
    }
}
