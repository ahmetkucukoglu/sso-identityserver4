namespace SSO.Application.Infrastructure.AspNet.Extensions
{
    using Exceptions;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Newtonsoft.Json;
    using System;

    public static class TempDataDictionaryExtensions
    {
        public static void AddTempDataIfUserFriendlyException(this ITempDataDictionary tempData, Exception exception)
        {
            if (exception is UserFriendlyException userFriendlyException)
            {
                tempData["UserFriendlyFailures"] = JsonConvert.SerializeObject(userFriendlyException.Failures);
            }
            else if (exception is ValidationException validationException)
            {
                tempData["ValidationFailures"] = JsonConvert.SerializeObject(validationException.Failures);
            }
        }
    }
}
