using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApplicationHub.Modules.Extensions
{
    public static class ValidationResultExtensions
    {
        public static void AddErrorsToModelState(this ValidationResult validationResult, ModelStateDictionary modelState)
        {
            foreach (var error in validationResult.Errors)
            {
                // If the field already exists in ModelState, add the error
                if (modelState.ContainsKey(error.PropertyName))
                {
                    modelState[error.PropertyName]!.Errors.Add(error.ErrorMessage);
                    modelState[error.PropertyName]!.ValidationState = ModelValidationState.Invalid;
                }
                else
                {
                    // Otherwise, create a new entry in ModelState
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }
    }
}
