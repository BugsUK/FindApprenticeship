namespace SFA.Apprenticeships.Web.Recruit.Providers
{
    using ViewModels.Application;

    public interface IApplicationProvider
    {
        VacancyApplicationsViewModel GetVacancyApplicationsViewModel(long vacancyReferenceNumber);
    }
}