namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using Common.Mediators;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

    public interface IVacancyPostingMediator
    {
        MediatorResponse<EmployerResultsViewModel> GetProviderEmployers(Guid providerId, EmployerFilterViewModel employerFilterViewModel);

        MediatorResponse<EmployerResultsViewModel> GetEmployers(EmployerSearchViewModel employerFilterViewModel);

        MediatorResponse<NewVacancyViewModel> GetNewVacancyModel(string username);

        MediatorResponse<NewVacancyViewModel> CreateVacancy(NewVacancyViewModel newVacancyViewModel);

        MediatorResponse<VacancyViewModel> GetVacancyViewModel(long vacancyReferenceNumber);

        MediatorResponse<VacancyViewModel> SubmitVacancy(VacancyViewModel viewModel);

        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(long vacancyReferenceNumber);
    }
}