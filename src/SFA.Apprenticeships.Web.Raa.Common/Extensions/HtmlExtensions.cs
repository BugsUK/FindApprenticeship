namespace SFA.Apprenticeships.Web.Raa.Common.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using ViewModels.Vacancy;
    using Web.Common.Validators.Extensions;

    public static class HtmlExtensions
    {
        public static ValidationResultViewModel GetValidationResultViewModel<TModel, TProperty>(this HtmlHelper<TModel> html, VacancyViewModel vacancyViewModel, Expression<Func<TModel, TProperty>> propertyExpression, ModelStateDictionary modelState, string viewWarningUrl, string comment)
        {
            var propertyName = ExpressionHelper.GetExpressionText(propertyExpression);

            var anchorName = propertyName.Replace(".", "_").ToLower();
            var hasError = modelState.HasErrorsFor(propertyName);
            var error = modelState.ErrorMessageFor(propertyName);
            var hasWarning = modelState.HasWarningsFor(propertyName);
            var warning = modelState.WarningMessageFor(propertyName);
            viewWarningUrl = $"{viewWarningUrl}#{propertyName.Substring(propertyName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower()}";

            var validationResultViewModel = new ValidationResultViewModel(vacancyViewModel.Status, anchorName, hasError, error, hasWarning, warning, viewWarningUrl, comment);

            return validationResultViewModel;
        }

        public static CommentViewModel GetCommentViewModel<TModel, TProperty>(this HtmlHelper<TModel> html, VacancyViewModel vacancyViewModel, Expression<Func<TModel, TProperty>> propertyExpression, string comment, string viewCommentUrl)
        {
            var propertyName = ExpressionHelper.GetExpressionText(propertyExpression);

            viewCommentUrl = $"{viewCommentUrl}#{propertyName.Substring(propertyName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower()}";

            var commentViewModel = new CommentViewModel(vacancyViewModel.Status, comment, viewCommentUrl, string.Empty);

            return commentViewModel;
        }

        public static CommentViewModel GetCommentViewModel<TModel, TProperty>(this HtmlHelper<TModel> html, VacancyViewModel vacancyViewModel, Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var commentLabelText = html.DisplayNameFor(propertyExpression).ToString();
            var commentText = html.ValueFor(propertyExpression).ToString();

            var commentViewModel = new CommentViewModel(vacancyViewModel.Status, commentText, string.Empty, commentLabelText);

            return commentViewModel;
        }

        public static CommentViewModel GetCommentViewModel<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var commentLabelText = html.DisplayNameFor(propertyExpression).ToString();
            var commentText = html.ValueFor(propertyExpression).ToString();

            var commentViewModel = new CommentViewModel(commentText, string.Empty, commentLabelText);

            return commentViewModel;
        }

        public static EditLinkViewModel GetEditLinkViewModel<TModel, TProperty>(this HtmlHelper<TModel> html, VacancyViewModel vacancyViewModel, Expression<Func<TModel, TProperty>> propertyExpression, string editUrl, string comment)
        {
            var propertyName = ExpressionHelper.GetExpressionText(propertyExpression);

            editUrl = $"{editUrl}#{propertyName.Substring(propertyName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower()}";

            var editLinkViewModel = new EditLinkViewModel(vacancyViewModel.Status, editUrl, comment);

            return editLinkViewModel;
        }

        public static object GetLabelFor<TModel, TProperty>(this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> propertyCommentExpression, string commentLabelText = null)
        {
            commentLabelText = commentLabelText ?? html.DisplayNameFor(propertyCommentExpression).ToString();
            return new {Label = commentLabelText};
        }
    }
}