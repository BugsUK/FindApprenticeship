namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application.Apprenticeship
{
    using FluentValidation.Attributes;
    using Validators.Application;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModel
    {
        public string NextStepsUrl { get; set; }
        public string ProviderName { get; set; }
        public string Contact { get; set; }
    }
}
