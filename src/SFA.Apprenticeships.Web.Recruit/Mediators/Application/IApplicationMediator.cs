namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    using Common.Mediators;
    using ViewModels.Application;

    public interface IApplicationMediator
    {
        MediatorResponse<VacancyApplicationsViewModel> GetVacancyApplicationsViewModel(long vacancyReferenceNumber);
    }
}