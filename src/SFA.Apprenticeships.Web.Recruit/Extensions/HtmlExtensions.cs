namespace SFA.Apprenticeships.Web.Recruit.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Common.Validators.Extensions;
    using ViewModels;

    public static class HtmlExtensions
    {
        public static ValidationResultViewModel GetValidationResultViewModelFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> propertyExpression, ModelStateDictionary modelState, string viewWarningUrl, string comment)
        {
            var propertyName = ExpressionHelper.GetExpressionText(propertyExpression);

            var hasError = modelState.HasErrorsFor(propertyName);
            var error = modelState.ErrorMessageFor(propertyName);
            var hasWarning = modelState.HasWarningsFor(propertyName);
            var warning = modelState.WarningMessageFor(propertyName);

            var validationResultViewModel = new ValidationResultViewModel(hasError, error, hasWarning, warning, viewWarningUrl, comment);

            return validationResultViewModel;
        }
    }
}