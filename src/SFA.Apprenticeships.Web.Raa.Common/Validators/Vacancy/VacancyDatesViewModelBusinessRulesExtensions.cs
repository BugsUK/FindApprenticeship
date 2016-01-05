namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Vacancy
{
    using Constants.ViewModels;
    using FluentValidation.Results;
    using ViewModels.Vacancy;
    using Web.Common.Validators;
    using Web.Common.ViewModels;

    public static class VacancyDatesViewModelBusinessRulesExtensions
    {
        public static ValidationFailure PossibleStartDateShouldBeAfterClosingDate(this VacancyDatesViewModel viewModel, DateViewModel closingDate, string parentPropertyName)
        {
            if (closingDate == null || !closingDate.HasValue || viewModel.PossibleStartDate == null || !viewModel.PossibleStartDate.HasValue || !Common.BeValidDate(closingDate) || !Common.BeValidDate(viewModel.PossibleStartDate))
            {
                return null;
            }

            if (viewModel.PossibleStartDate.Date <= closingDate.Date)
            {
                var propertyName = "PossibleStartDate";
                if (!string.IsNullOrEmpty(parentPropertyName))
                {
                    propertyName = parentPropertyName + "." + propertyName;
                }
                var validationFailure = new ValidationFailure(propertyName, VacancyViewModelMessages.PossibleStartDate.BeforePublishDateErrorText)
                {
                    CustomState = ValidationType.Warning
                };
                return validationFailure;
            }

            return null;
        }
    }
}