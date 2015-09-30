namespace SFA.Apprenticeships.Web.Recruit.Mediators.VacancyPosting
{
    using System;
    using Common.Mediators;
    using ViewModels.Vacancy;

    public interface IVacancyPostingMediator
    {
        NewVacancyViewModel Index(string johnDoeExampleCom);
        MediatorResponse<SubmittedVacancyViewModel> GetSubmittedVacancyViewModel(Guid id);
    }
}