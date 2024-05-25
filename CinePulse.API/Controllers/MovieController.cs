using CinePulse.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CinePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return BadRequest("No search criteria provided.");
            }
            return Ok(await _movieService.GetMovieByTitleAsync(title));
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetSearchHistory()
        {
            return Ok(await _movieService.GetSearchHistoryAsync());
        }
    }
}
