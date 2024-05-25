using CinePulse.Application.DTOs;
using CinePulse.Application.Services.Interface;
using CinePulse.Domain;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CinePulse.Application.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly string _apiKey = "899ec4f7";
        private static readonly List<string> _searchHistory = [];
        private static readonly object _lock = new();

        public async Task<ApiResponse<MovieResponseDto>> GetMovieByTitleAsync(string title)
        {
            AddToSearchHistory(title);
            using var client = new HttpClient();
            try
            {
                var response = await client.GetStringAsync($"http://www.omdbapi.com/?apikey={_apiKey}&t={title}");
                var result = JsonConvert.DeserializeObject<MovieResponseDto>(response);
                return new ApiResponse<MovieResponseDto>(true, StatusCodes.Status200OK, "Movies retrieved successfully", result);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<MovieResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while processing your request", new List<string> { ex.Message });
            }
            catch (JsonException ex)
            {
                return new ApiResponse<MovieResponseDto>(false, StatusCodes.Status500InternalServerError, "Error parsing the movie data", new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return new ApiResponse<MovieResponseDto>(false, StatusCodes.Status500InternalServerError, "An unexpected error occurred", new List<string> { ex.Message });
            }
        }

        public Task<ApiResponse<IEnumerable<string>>> GetSearchHistoryAsync()
        {
            try
            {
                lock (_lock)
                {
                    if (_searchHistory.Count < 1)
                    {
                        return Task.FromResult(new ApiResponse<IEnumerable<string>>(false, StatusCodes.Status404NotFound, "Search history not found"));
                    }

                    var result = _searchHistory.Take(5).ToList();
                    return Task.FromResult(new ApiResponse<IEnumerable<string>>(true, StatusCodes.Status200OK, "Search history retrieved successfully", result, null));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ApiResponse<IEnumerable<string>>(false, StatusCodes.Status500InternalServerError, "An error occurred while processing your request", new List<string> { ex.Message }));
            }
        }

        private static void AddToSearchHistory(string title)
        {
            lock (_lock)
            {
                _searchHistory.Insert(0, title);
                if (_searchHistory.Count > 5)
                {
                    _searchHistory.RemoveAt(5);
                }
            }
        }
    }
}
