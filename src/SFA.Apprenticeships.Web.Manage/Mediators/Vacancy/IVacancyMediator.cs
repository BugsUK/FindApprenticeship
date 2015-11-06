namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using Common.Mediators;
    using ViewModels;

    public interface IVacancyMediator
    {
        MediatorResponse<VacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber);
    }
}