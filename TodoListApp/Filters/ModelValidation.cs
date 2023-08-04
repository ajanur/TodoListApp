using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.ServiceExtension;

namespace TodoListApp.Filters
{
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        ILoggerManager _logger = new LoggerManager();
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("Model Validation");
            if (!context.ModelState.IsValid)
            {
                _logger.LogError("Model validation error");
                context.Result = new BadRequestObjectResult(context.ModelState); // returns 400 with error
            }
        }
    }
}
