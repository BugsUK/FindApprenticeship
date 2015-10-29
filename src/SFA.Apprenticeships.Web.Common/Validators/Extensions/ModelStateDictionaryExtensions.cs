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

        public static bool HasWarnings(this ModelStateDictionary modelStateDictionary)
        {
            return modelStateDictionary.Values.Any(modelState => modelState.Errors.Any(e => e.GetType() == typeof(ModelWarning)));
        }
    }
}