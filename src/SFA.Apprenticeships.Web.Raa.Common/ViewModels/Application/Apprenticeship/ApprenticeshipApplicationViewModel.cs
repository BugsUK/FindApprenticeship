namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application.Apprenticeship
{
    using FluentValidation.Attributes;
    using Validators.Application;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModel
    {
    }
}
