namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyStatus
{
    using Common.Mediators;
    using ViewModels.VacancyStatus;

    public interface IVacancyStatusMediator
    {
        MediatorResponse<ArchiveVacancyViewModel> GetArchiveVacancyViewModel(int vacancyReferenceNumber);
    }
}
