using System;

namespace Api.Shared.Exceptions
{
    public class AppValidationException: AppException
    {
        public string Identifier { get; set; }
        public AppValidationException(string identifier, string message)
            :base(message)
        {
            this.Identifier = identifier;
        }
    }
}