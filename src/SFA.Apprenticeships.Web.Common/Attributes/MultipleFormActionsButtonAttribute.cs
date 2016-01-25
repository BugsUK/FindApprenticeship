namespace SFA.Apprenticeships.Web.Common.Attributes
{
    using System;
    using System.Linq;
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

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleFormActionsButtonWithParameterAttribute : ActionNameSelectorAttribute
    {
        public string SubmitButtonActionName { get; set; }
        
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var value = controllerContext.Controller.ValueProvider.GetValue(SubmitButtonActionName);
            string realActionName;
            var isValid = false;

            if (value == null)
            {
                realActionName = actionName.Split('-').First();
                isValid = methodInfo.Name.Equals(realActionName, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                realActionName = value.AttemptedValue.Split('-').First();
                isValid = realActionName == methodInfo.Name;
            }


            return isValid;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FillParamterFromActionName : ActionFilterAttribute
    {
        public string[] ParameterNames { get; set; }

        public string SubmitButtonActionName { get; set; }

        public TypeCode[] ParameterTypes { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var value = filterContext.HttpContext.Request.Params[SubmitButtonActionName];
            var splittedParameterValue = value.Split('-');
            if (splittedParameterValue.Length > 1)
            {
                for (var i = 1; i < splittedParameterValue.Length; i++)
                {
                    var parameterValue = splittedParameterValue[i];
                    var realParameterValue = Convert.ChangeType(parameterValue, ParameterTypes[i - 1]);
                    filterContext.ActionParameters[ParameterNames[i - 1]] = realParameterValue;
                }

            }
            else
            {
                filterContext.ActionParameters[ParameterNames[0]] = null;
            }
        }
    }
}