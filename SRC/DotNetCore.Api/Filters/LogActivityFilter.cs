using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace DotNetCore_WebApi.Filters
{
    public class LogActivityFilter : IActionFilter,IAsyncActionFilter
    {
        private readonly ILogger<LogActivityFilter> _logger;

        public LogActivityFilter(ILogger<LogActivityFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} finished execution on controller {context.Controller} with arguments {JsonSerializer.Serialize(context.ActionArguments)}");



            //Short circuit
            //Do not complete
            //context.Result = new NotFoundResult();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Execution Action {context.ActionDescriptor.DisplayName} on controller {context.Controller}");
        }

        //Have periority than IActionFilters Actions
        //هيتنفذ والاتنين اللي فوق لا
        //Perfered to use this
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"(Async) Action {context.ActionDescriptor.DisplayName} finished execution on controller {context.Controller} with arguments {JsonSerializer.Serialize(context.ActionArguments)}");

            await next();

            _logger.LogInformation($"(Async) Execution Action {context.ActionDescriptor.DisplayName} on controller {context.Controller}");

        }
    }
}
