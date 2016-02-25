namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Application
{
    using System;

    public class ApplicationSelectionViewModel : VacancyApplicationsSearchViewModel
    {
        public ApplicationSelectionViewModel()
        {
            
        }

        public ApplicationSelectionViewModel(VacancyApplicationsSearchViewModel viewModel, Guid applicationId)
            : base(viewModel)
        {
            ApplicationId = applicationId;
        }

        public ApplicationSelectionViewModel(ApplicationSelectionViewModel viewModel) 
            : this(viewModel, viewModel.ApplicationId)
        {

        }

        public Guid ApplicationId { get; }
    }
}
