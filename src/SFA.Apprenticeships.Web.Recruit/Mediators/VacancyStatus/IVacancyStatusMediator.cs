namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Application.Apprenticeship;
    using Raa.Common.ViewModels.VacancyStatus;

    public interface IVacancyStatusMediator
    {
        MediatorResponse<ArchiveVacancyViewModel> GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber);

        MediatorResponse<ArchiveVacancyViewModel> ArchiveVacancy(ArchiveVacancyViewModel viewModel);
        MediatorResponse<BulkDeclineCandidatesViewModel> GetBulkDeclineCandidatesViewModelByVacancyReferenceNumber(int vacancyReferenceNumber);
        MediatorResponse<BulkDeclineCandidatesViewModel> BulkResponseApplications(BulkApplicationsRejectViewModel bulkApplicationsRejectViewModel);
    }
}
