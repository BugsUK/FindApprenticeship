namespace SFA.Apprenticeship.Api.AvService.Validators
{
    using System.Collections.Generic;
    using System.Linq;
    using DataContracts.Version51;
    using FluentValidation;
    using FluentValidation.Results;

    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, ErrorCodesData error)
        {
            return rule.WithState(obj => error);
        }

        public static List<ErrorCodesData> GetErrorCodes(this ValidationResult validationResult)
        {
            if (validationResult?.Errors == null)
            {
                return Enumerable
                    .Empty<ErrorCodesData>()
                    .ToList();
            }

            return validationResult
                .Errors
                .Select(error => error.CustomState)
                .Cast<ErrorCodesData>()
                .ToList();
        }
    }
}
