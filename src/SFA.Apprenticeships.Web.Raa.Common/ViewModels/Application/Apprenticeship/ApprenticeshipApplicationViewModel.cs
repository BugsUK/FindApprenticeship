namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application.Apprenticeship
{
    using FluentValidation.Attributes;
    using System.Collections.Generic;
    using Validators.Application;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModel
    {
        public string NextStepsUrl { get; set; }

        public IList<BulkRejectApplication> BulkRejectApplications { get; set; }
    }

    public class BulkRejectApplication
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ApplicationId { get; set; }
    }
}
