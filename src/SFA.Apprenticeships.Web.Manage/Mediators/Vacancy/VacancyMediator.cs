using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

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

        public MediatorResponse<DashboardVacancySummaryViewModel> ApproveVacancy(long vacancyReferenceNumber)
        {
            _vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            var vacancies = _vacancyProvider.GetPendingQAVacancies();

            if (vacancies == null || !vacancies.Any())
            {
                return GetMediatorResponse<DashboardVacancySummaryViewModel>(VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies);
            }

            return GetMediatorResponse(VacancyMediatorCodes.ApproveVacancy.Ok, vacancies.First());
        }

        public MediatorResponse<VacancyViewModel> GetVacancy(long vacancyReferenceNumber)
        {
            var vacancyViewModel = _vacancyProvider.GetVacancy(vacancyReferenceNumber);
            
            if (vacancyViewModel == null)
            {
                return GetMediatorResponse<VacancyViewModel>(VacancyMediatorCodes.GetVacancy.NotAvailable);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancy.Ok, vacancyViewModel);
        }
    }
}