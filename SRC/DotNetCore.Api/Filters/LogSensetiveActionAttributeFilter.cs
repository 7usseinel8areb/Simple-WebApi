using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace DotNetCore_WebApi.Filters
{
    public class LogSensetiveActionAttributeFilter:ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //context.HttpContext

            Debug.WriteLine("Sensetive Action Executed !!!!!!!!!");
            
        }
    }
}
