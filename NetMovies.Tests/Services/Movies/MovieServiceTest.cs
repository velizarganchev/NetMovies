namespace NetMovies.Tests.Services.Movies
{
    using NetMovies.Data.Models;
    using NetMovies.Services.Movies;
    using NetMovies.Services.Statistics;
    using NetMovies.Tests.Mocks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    public class MovieServiceTest
    {

        [Fact]
        public void ShoudReturnCorrectMovieQueryServiceModel()
        {
            //Arrange
            using var data = DatabaseMock.Instance;

            data.Movies.AddRange(new[] { new Movie { MovieId = 111,  }, new Movie { MovieId = 112 } });
            data.SaveChanges();
         
            var mapper = MapperMock.Instance;

            var movieService = new MovieService(data, null, mapper);

            //Act

            var result = movieService.Index();

            //Assert

            //Assert.Equal(2, result.Movies.Count());
        }


        [Fact]
        public void ShoudReturnCorrectCategorisCount()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            data.Genres.AddRange(new[]
            {
                new Genre { GenreId = 111, GenreName = "Action" },
                new Genre { GenreId = 112, GenreName = "Drama" } });
            data.SaveChanges();

            var movieService = new MovieService(data, null, mapper);

            //Act

            var result = movieService.GenreCategories().Count();

            //Assert

            Assert.Equal(2, result);
        }
        [Fact]
        public void ShoudReturnCorrectQualitiesCount()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            data.Qualities.AddRange(new[]
            {
                new Quality { QualityId = 111, QualityName = "HD" },
                new Quality { QualityId = 112, QualityName = "720px" } });
            data.SaveChanges();

            var movieService = new MovieService(data, null, mapper);

            //Act

            var result = movieService.Qualities().Count();

            //Assert

            Assert.Equal(2, result);
        }
        [Fact]
        public void ShoudReturnCorrectTrueWhenGenreExist()
        {
            //Arrange
            using var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            data.Genres.Add(new Genre { GenreId = 111, GenreName = "HD" });
            data.SaveChanges();

            var movieService = new MovieService(data, null, mapper);

            //Act

            var result = movieService.GenreExists(111);

            //Assert

            Assert.True(result);
        }
    }
}
