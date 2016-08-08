namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using ViewModels.VacancyStatus;

    public interface IVacancyStatusChangeProvider
    {
        ArchiveVacancyViewModel GetArchiveVacancyViewModelByVacancyReferenceNumber(int vacancyReferenceNumber);

        ArchiveVacancyViewModel ArchiveVacancy(ArchiveVacancyViewModel viewModel);
    }
}
