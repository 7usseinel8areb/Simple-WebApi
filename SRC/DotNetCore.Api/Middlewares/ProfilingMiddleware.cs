using System.Diagnostics;

namespace DotNetCore_WebApi.Middlewares
{
    //This Middleware calculating the time token by the request to execute
    public class ProfilingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProfilingMiddleware> _logger;

        //First Constraint inject this in the constructor of your middleware
        public ProfilingMiddleware(RequestDelegate next,ILogger<ProfilingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation($"Request `{context.Request.Path}` took {stopwatch.ElapsedMilliseconds}ms to exxecute");

        }
    }
}
