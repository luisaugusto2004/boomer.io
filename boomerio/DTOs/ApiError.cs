namespace boomerio.DTOs
{
    public class ApiError
    {
        public string Type { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public ApiError(string type, int status, string message)
        {
            Type = type;
            Status = status;
            Message = message;
        }
    }
}
