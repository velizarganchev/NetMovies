namespace NetMovies.Tests
{
    using Xunit;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("2", 2.ToString());
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        [InlineData(2, 2, 4)]
        public void Test2(int x, int y, int sum)
        {
            Assert.Equal(sum, x + y);
        }
    }
}
