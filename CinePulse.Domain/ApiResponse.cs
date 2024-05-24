namespace CinePulse.Domain
{
    public class ApiResponse<T>
    {
        public bool IsSuceeded { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse(bool isSuceeded, int statusCode, string message, T data, List<string> errors)
        {
            IsSuceeded = isSuceeded;
            StatusCode = statusCode;
            Message = message;
            Data = data;
            Errors = errors;
        }

        public ApiResponse(bool isSuceeded, int statusCode, string message, T data)
        {
            IsSuceeded = isSuceeded;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public ApiResponse(bool isSuceeded, int statusCode, string message, List<string> errors)
        {
            IsSuceeded = isSuceeded;
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        public ApiResponse(bool isSuceeded, int statusCode, string message)
        {
            IsSuceeded = isSuceeded;
            StatusCode = statusCode;
            Message = message;
        }
    }
}
