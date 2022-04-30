namespace NetMovies.Tests.Controllers
{
    using NetMovies.Controllers;
    using NetMovies.Data.Models;
    using NetMovies.Models.Movie;
    using NetMovies.Services.Movies;
    using NetMovies.Tests.Mocks;
    using Shouldly;
    using System;
    using System.Linq;
    using Xunit;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexWithoutSearchTermShouldReturnCorrectViewWithModel()
        {

            //Arrange
           using var data = DatabaseMock.Instance;


            //var movieService = new MovieService();
        }
    }
}
