namespace SFA.Apprenticeships.Web.Common.Validators.Extensions
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;
    using FluentValidation.Results;

    public static class ValidationResultExtension
    {
        public static void AddToModelStateWithSeverity(this ValidationResult result, ModelStateDictionary modelState, string prefix)
        {
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    var key = string.IsNullOrEmpty(prefix) ? error.PropertyName : prefix + "." + error.PropertyName;
                    if ((ValidationType?) error.CustomState == ValidationType.Warning)
                    {
                        GetModelStateForKey(modelState, key).Errors.Add(new ModelWarning(error.ErrorMessage));
                    }
                    else
                    {
                        modelState.AddModelError(key, error.ErrorMessage);
                        //To work around an issue with MVC: SetModelValue must be called if AddModelError is called.
                        modelState.SetModelValue(key, new ValueProviderResult(error.AttemptedValue ?? "", (error.AttemptedValue ?? "").ToString(), CultureInfo.CurrentCulture));
                    }
                }
            }
        }

        private static ModelState GetModelStateForKey(ModelStateDictionary modelState, string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            ModelState keyModelState;
            if (!modelState.TryGetValue(key, out keyModelState))
            {
                keyModelState = new ModelState();
                modelState[key] = keyModelState;
            }

            return keyModelState;
        }
    }
}