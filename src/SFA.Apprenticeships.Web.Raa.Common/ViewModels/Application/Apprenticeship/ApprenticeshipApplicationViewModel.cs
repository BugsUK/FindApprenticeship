namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application.Apprenticeship
{
    using System;
    using Domain.Entities.Applications;
    using FluentValidation.Attributes;
    using Validators.Application;

    [Validator(typeof(ApprenticeshipApplicationViewModelClientValidator))]
    public class ApprenticeshipApplicationViewModel : ApplicationViewModel
    {
        public string NextStepsUrl { get; set; }
        public Guid CandidateId { get; set; }
        public string ProviderName { get; set; }
        public string Contact { get; set; }
        public string UnsuccessfulReason { get; set; }
        public ApplicationStatuses ApplicationStatus { get; set; }

        public string ApplicationStatusDescription
        {
            get
            {
                switch (ApplicationStatus)
                {
                    case ApplicationStatuses.Draft:
                        return "Draft";

                    case ApplicationStatuses.Submitted:
                    case ApplicationStatuses.Submitting:
                        return "Submitted";

                    case ApplicationStatuses.InProgress:
                        return "In progress";

                    case ApplicationStatuses.ExpiredOrWithdrawn:
                        return "Expired or withdrawn";

                    case ApplicationStatuses.Successful:
                        return "Successful";

                    case ApplicationStatuses.Unsuccessful:
                        return "Unsuccessful";
                }

                return string.Empty;
            }
        }

        public string Title { get; set; }
        public string EmployerName { get; set; }
    }
}
