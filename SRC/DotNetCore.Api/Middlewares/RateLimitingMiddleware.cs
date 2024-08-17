namespace DotNetCore_WebApi.Middlewares
{
    // This middleware sets a maximum request limit to protect against DDOS attacks.
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static DateTime _lastRequest = DateTime.Now;
        private static int _counter = 0;

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _counter++;
            if(DateTime.Now.Subtract( _lastRequest).Seconds > 10)
            {
                _counter = 1;
                _lastRequest = DateTime.Now;

                await _next(context);
            }
            else
            {
                if(_counter > 5)
                {
                    _lastRequest = DateTime.Now;
                    await context.Response.WriteAsync("Rate Limit exceeded");
                }
                else
                {
                    _lastRequest = DateTime.Now;
                    await _next(context);
                }
            }
        }
    }
}
