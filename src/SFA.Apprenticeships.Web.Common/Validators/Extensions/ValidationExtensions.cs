namespace SFA.Apprenticeships.Web.Common.Validators.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class ValidationExtensions
    {
        public static MvcHtmlString ValidationMessageWithSeverityFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ValidationType validationType)
        {
            return ValidationMessageWithSeverityFor(htmlHelper, expression, null, validationType);
        }

        public static MvcHtmlString ValidationMessageWithSeverityFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage, ValidationType validationType)
        {
            var validationMessageFor = htmlHelper.ValidationMessageFor(expression, validationMessage, new { @class = "error-message" });
            
            if (validationType == ValidationType.Warning)
            {
                var validationWarningMessageFor = validationMessageFor.ToString()
                    .Replace(HtmlHelper.ValidationMessageCssClassName,
                        Validators.HtmlHelper.ValidationWarningMessageCssClassName);

                return new MvcHtmlString(validationWarningMessageFor);
            }

            return validationMessageFor;
        }
    }
}