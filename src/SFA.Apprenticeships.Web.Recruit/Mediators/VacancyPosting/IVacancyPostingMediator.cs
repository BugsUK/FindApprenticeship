namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using Common.Mediators;
    using ViewModels.Vacancy;

    public interface IVacancyPostingMediator
    {
        MediatorResponse<NewVacancyViewModel> GetNewVacancyModel(string username);

        MediatorResponse<VacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel);

        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber);
    }
}