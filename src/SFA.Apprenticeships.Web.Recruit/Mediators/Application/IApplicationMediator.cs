namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Application;
    using System.Web.Mvc;

    public interface IApplicationMediator
    {
        MediatorResponse<VacancyApplicationsViewModel> GetVacancyApplicationsViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearch);
        MediatorResponse<ShareApplicationsViewModel> ShareApplications(int vacancyReferenceNumber);
        MediatorResponse<ShareApplicationsViewModel> ShareApplications(ShareApplicationsViewModel vacancyReferenceNumber, UrlHelper urlHelper);

        MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(int vacancyReferenceNumber);
        MediatorResponse<BulkDeclineCandidatesViewModel> BulkResponseApplications(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel);
        MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModel(VacancyApplicationsSearchViewModel vacancyApplicationsSearchViewModel);
        BulkApplicationsRejectViewModel GetApplicationViewModel(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel);
    }

}