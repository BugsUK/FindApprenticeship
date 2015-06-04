namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleFormActionsButtonAttribute : ActionNameSelectorAttribute
    {
        public string SubmitButtonActionName { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var value = controllerContext.Controller.ValueProvider.GetValue(SubmitButtonActionName);

            if (value == null)
            {
                return methodInfo.Name.Equals(actionName, StringComparison.InvariantCultureIgnoreCase);
            }

            return value.AttemptedValue == methodInfo.Name;
        }
    }
}