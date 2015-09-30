namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using Common.Mediators;
    using ViewModels.Vacancy;

    public interface IVacancyPostingMediator
    {
        NewVacancyViewModel Index(string johnDoeExampleCom);
        MediatorResponse<VacancyViewModel> CreateVacancy(NewVacancyViewModel viewModel);
        MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber);
        MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel);
        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber);
    }
}