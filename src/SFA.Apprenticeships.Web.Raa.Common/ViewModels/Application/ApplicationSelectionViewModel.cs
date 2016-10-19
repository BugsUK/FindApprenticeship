namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using Domain.Entities.Applications;
    using System;
    using System.Collections.Generic;

    public class ApplicationSelectionViewModel : VacancyApplicationsSearchViewModel
    {
        public ApplicationSelectionViewModel()
        {

        }

        public ApplicationSelectionViewModel(Guid applicationId, int vacancyReferenceNumber) : base(vacancyReferenceNumber)
        {
            ApplicationId = applicationId;
        }

        public ApplicationSelectionViewModel(VacancyApplicationsSearchViewModel viewModel, Guid applicationId) : base(viewModel)
        {
            ApplicationId = applicationId;
        }

        public ApplicationSelectionViewModel(ApplicationSelectionViewModel viewModel) : this(viewModel, viewModel.ApplicationId)
        {

        }

        public Guid ApplicationId { get; set; }

        public IList<ApprenticeshipApplicationDetail> BulkDeclineApplications { get; set; }

        public override object RouteValues => new
        {
            ApplicationId,
            VacancyReferenceNumber,
            FilterType,
            ApplicantId,
            FirstName,
            LastName,
            Postcode,
            OrderByField,
            Order,
            PageSize,
            CurrentPage
        };
    }
}
