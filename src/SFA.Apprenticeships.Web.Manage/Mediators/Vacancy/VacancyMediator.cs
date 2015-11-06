namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System.Linq;
    using Common.Mediators;
    using Providers;
    using ViewModels;

    public class VacancyMediator : MediatorBase, IVacancyMediator
    {
        private readonly IVacancyProvider _vacancyProvider;

        public VacancyMediator(IVacancyProvider vacancyProvider)
        {
            _vacancyProvider = vacancyProvider;
        }

        public MediatorResponse<VacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber)
        {
            _vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            var vacancies = _vacancyProvider.GetPendingQAVacancies();

            if (vacancies == null || !vacancies.Any())
            {
                return GetMediatorResponse<VacancySummaryViewModel>(VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies);
            }

            return GetMediatorResponse(VacancyMediatorCodes.ApproveVacancy.Ok, vacancies.First());
        }
    }
}