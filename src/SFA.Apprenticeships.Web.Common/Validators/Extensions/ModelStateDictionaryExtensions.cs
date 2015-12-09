namespace SFA.Apprenticeships.Web.Common.Validators.Extensions
{
    using System.Linq;
    using System.Web.Mvc;

    public static class ModelStateDictionaryExtensions
    {
        public static bool HasErrors(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.Any(modelState => modelState.Errors.Any(e => e.GetType() == typeof(ModelError)));
        }

        public static bool HasErrorsFor(this ModelStateDictionary modelStateDictionary, string fieldKey)
        {
            var exists = false;

            foreach (var modelKey in modelStateDictionary.Keys)
            {
                foreach (var modelError in modelStateDictionary[modelKey].Errors)
                {
                    if (modelError.GetType() == typeof(ModelError))
                    {
                        if (modelKey == fieldKey)
                        {
                            exists = true;
                        }
                    }
                }
            }

            return exists;
        }

        public static string ErrorMessageFor(this ModelStateDictionary modelStateDictionary, string fieldKey)
        {
            var warningMessage = string.Empty;

            foreach (var modelKey in modelStateDictionary.Keys)
            {
                foreach (var modelError in modelStateDictionary[modelKey].Errors)
                {
                    if (modelError.GetType() == typeof(ModelError))
                    {
                        if (modelKey == fieldKey)
                        {
                            warningMessage = modelError.ErrorMessage;
                        }
                    }
                }
            }

            return warningMessage;
        }

        public static bool HasWarnings(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.Any(modelState => modelState.Errors.Count(e => e.GetType() == typeof(ModelWarning)) > 0 && modelState.Errors.All(e => e.GetType() == typeof(ModelWarning)));
        }

        public static bool HasWarningsFor(this ModelStateDictionary modelStateDictionary, string fieldKey)
        {
            var exists = false;

            foreach (var modelKey in modelStateDictionary.Keys)
            {
                foreach (var modelError in modelStateDictionary[modelKey].Errors)
                {
                    if (modelError.GetType() == typeof (ModelWarning))
                    {
                        if (modelKey == fieldKey)
                        {
                            exists = true;
                        }
                    }
                }
            }

            return exists;
        }

        public static string WarningMessageFor(this ModelStateDictionary modelStateDictionary, string fieldKey)
        {
            var warningMessage = string.Empty;

            foreach (var modelKey in modelStateDictionary.Keys)
            {
                foreach (var modelError in modelStateDictionary[modelKey].Errors)
                {
                    if (modelError.GetType() == typeof(ModelWarning))
                    {
                        if (modelKey == fieldKey)
                        {
                            warningMessage = modelError.ErrorMessage;
                        }
                    }
                }
            }

            return warningMessage;
        }
    }
}