namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System;

    public class ApplicationSelectionViewModel : VacancyApplicationsSearchViewModel
    {
        public ApplicationSelectionViewModel()
        {
            
        }

        public ApplicationSelectionViewModel(VacancyApplicationsSearchViewModel viewModel, Guid applicationId) : base(viewModel)
        {
            ApplicationId = applicationId;
        }

        public ApplicationSelectionViewModel(ApplicationSelectionViewModel viewModel) : this(viewModel, viewModel.ApplicationId)
        {

        }

        public Guid ApplicationId { get; set; }

        public override object RouteValues => new
        {
            ApplicationId,
            VacancyReferenceNumber,
            FilterType,
            OrderByField,
            Order,
            PageSize,
            CurrentPage
        };
    }
}
