namespace SSO.Application.Infrastructure.AspNet.Extensions
{
    using Exceptions;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;

    public static class ModelStateDictionaryExtensions
    {
        public static void AddModelErrorIfUserFriendlyException(this ModelStateDictionary modelState, Exception exception)
        {
            if (exception is UserFriendlyException userFriendlyException)
            {
                foreach (var failure in userFriendlyException.Failures)
                {
                    modelState.AddModelError("", failure);
                }
            }
        }
    }
}
