namespace SSO.Application.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class UserFriendlyException : Exception
    {
        public UserFriendlyException() : base("One or more validation failures have occurred.")
        {
            Failures = new List<string>();
        }

        public UserFriendlyException(List<string> failures) : this()
        {
            Failures = failures;
        }

        public UserFriendlyException(string failure) : this()
        {
            Failures.Add(failure);
        }

        public List<string> Failures { get; }
    }
}
