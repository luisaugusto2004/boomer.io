namespace boomerio.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                if (_env.IsDevelopment())
                {
                    var response = new
                    {
                        message = ex.Message,
                        stackTrace = ex.StackTrace
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    var response = new { message = "An unexpected error occurred. Please try again later." };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}
