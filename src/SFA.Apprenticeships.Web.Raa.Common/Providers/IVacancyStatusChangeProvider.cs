namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using ViewModels.VacancyStatus;

    public interface IVacancyStatusChangeProvider
    {
        ArchiveVacancyViewModel GetArchiveVacancyViewModelByVacancyId(int vacancyId);
    }
}
