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

    public class FillParamterFromActionName : ActionFilterAttribute
    {
        public string ParameterName { get; set; }

        public string SubmitButtonActionName { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var value = filterContext.HttpContext.Request.Params[SubmitButtonActionName];
            var parameterValue = value.Split('-').Last();

            var realParameterValue = Convert.ChangeType(parameterValue, TypeCode.Int32);
            filterContext.ActionParameters[ParameterName] = parameterValue;
            //base.OnActionExecuting(filterContext);
        }
    }
}