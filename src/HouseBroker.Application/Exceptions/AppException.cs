using System;

namespace Api.Shared.Exceptions
{
    public class AppException: Exception
    {
        public string Code { get; }
        public AppException(string message)
            :base(message)
        {
            this.Code = "1";
        }
    }
}