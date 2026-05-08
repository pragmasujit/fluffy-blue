using FluentValidation;
using HouseBroker.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace HouseBroker.Shared.Validation
{
    public abstract class DomainValidator<T> : AbstractValidator<T>
    {
        public void ValidateAndThrow(T instance)
        {
            var result = Validate(instance);
            if (!result.IsValid)
            {
                var firstError = result.Errors.First();

                var propertyName = Regex.Replace(firstError.PropertyName, @"\[\d+\]", "");

                throw new DomainValidationException(
                    identifier: propertyName,
                    message: firstError.ErrorMessage
                );
            }
        }
    }
}