using FluentValidation.Results;

namespace CusstomAuth.Core.ErrorHandling.Extensions;

public static class ValidationResultExtensions
{
    public static Dictionary<string, List<string>> MapToFailedValidation(this ValidationResult validationResult)
    {
        return validationResult.Errors.GroupBy(s => s.PropertyName).ToDictionary(s => s.Key, s => s.Select(s => s.ErrorMessage).ToList());
    }
}
