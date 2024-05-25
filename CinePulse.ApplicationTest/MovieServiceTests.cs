using CinePulse.Application.DTOs;
using CinePulse.Application.Services.Implementation;
using Newtonsoft.Json;
using System.Net;

namespace CinePulse.ApplicationTest
{
    [Collection("MovieServiceTests")]
    public class MovieServiceTests
    {
        private readonly string _apiKey = "899ec4f7";
        private MovieService _movieService;

        private HttpClient GetHttpClient(HttpResponseMessage responseMessage)
        {
            var handler = new CustomHttpMessageHandler((request, cancellationToken) => Task.FromResult(responseMessage));
            return new HttpClient(handler);
        }

        [Fact]
        public async Task GetMovieByTitleAsync_ReturnsMovie_WhenTitleIsValid()
        {
            // Arrange
            var title = "Inception";
            var movieResponse = new MovieResponseDto { Title = "Inception", Year = "2010" };
            var jsonResponse = JsonConvert.SerializeObject(movieResponse);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };

            var httpClient = GetHttpClient(responseMessage);
            _movieService = new MovieService(httpClient);

            // Act
            var response = await _movieService.GetMovieByTitleAsync(title);

            // Assert
            Assert.True(response.IsSuceeded);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Movies retrieved successfully", response.Message);
            Assert.Equal(movieResponse.Title, response.Data.Title);
        }

        [Fact]
        public async Task GetSearchHistoryAsync_ReturnsHistory_WhenHistoryExists()
        {
            // Arrange
            var title = "Inception";
            MovieService.AddToSearchHistory(title);
            var httpClient = GetHttpClient(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
            _movieService = new MovieService(httpClient);

            // Act
            var response = await _movieService.GetSearchHistoryAsync();

            // Assert
            Assert.True(response.IsSuceeded);
            Assert.Equal(200, response.StatusCode);
            Assert.Contains(title, response.Data);
        }

        // Test has to be run independently so as not to retain the history of search in the sequence of tests
        [Fact]
        public async Task GetSearchHistoryAsync_ReturnsNull_WhenNoHistoryExists()
        {
            // Arrange
            var httpClient = GetHttpClient(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
            _movieService = new MovieService(httpClient);

            // Act
            var response = await _movieService.GetSearchHistoryAsync();

            // Assert
            Assert.True(response.IsSuceeded);
            Assert.Equal(200, response.StatusCode);
            Assert.Equal("Search history is empty", response.Message);
            Assert.Null(response.Data);
        }
    }
}
