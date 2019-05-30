namespace SSO.Application.Infrastructure.AspNet
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class FriendlyExceptionHandlingActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            
            if (controller?.TempData["UserFriendlyFailures"] != null)
            {
                var failures = JsonConvert.DeserializeObject<List<string>>(controller.TempData["UserFriendlyFailures"].ToString());

                foreach (var failure in failures)
                {
                    context.ModelState.AddModelError("", failure);
                }
            }
            else if (controller?.TempData["ValidationFailures"] != null)
            {
                var failures = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(controller.TempData["ValidationFailures"].ToString());

                foreach (var failure in failures)
                {
                    foreach (var item in failure.Value)
                    {
                        context.ModelState.AddModelError(failure.Key, item);
                    }
                }
            }
        }
    }
}
