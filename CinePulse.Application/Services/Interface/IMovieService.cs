using CinePulse.Application.DTOs;
using CinePulse.Domain;

namespace CinePulse.Application.Services.Interface
{
    public interface IMovieService
    {
        Task<ApiResponse<MovieResponseDto>> GetMovieByTitleAsync(string title);
        Task<ApiResponse<IEnumerable<string>>> GetSearchHistoryAsync();
    }
}
