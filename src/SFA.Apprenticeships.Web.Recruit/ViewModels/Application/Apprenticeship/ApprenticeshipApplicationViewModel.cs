namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application.Apprenticeship
{
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModel
    {

    }
}