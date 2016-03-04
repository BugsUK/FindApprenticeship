namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application.Traineeship
{
    using FluentValidation.Attributes;
    using Validators.Application;

    [Validator(typeof(TraineeshipApplicationViewModelClientValidator))]
    public class TraineeshipApplicationViewModel : ApplicationViewModel
    {
         
    }
}