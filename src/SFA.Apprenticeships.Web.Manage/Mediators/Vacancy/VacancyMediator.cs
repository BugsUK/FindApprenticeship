namespace SFA.Apprenticeships.Web.Manage.Mediators.Vacancy
{
    using System.Linq;
    using Common.Mediators;
    using Providers;
    using FluentValidation;
    using ViewModels;
    using Common.Validators;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    public class VacancyMediator : MediatorBase, IVacancyMediator
    {
        private readonly IVacancyProvider _vacancyProvider;
        private readonly VacancyViewModelValidator _vacancyViewModelValidator;

        public VacancyMediator(IVacancyProvider vacancyProvider, VacancyViewModelValidator vacancyViewModelValidator)
        {
            _vacancyProvider = vacancyProvider;
            _vacancyViewModelValidator = vacancyViewModelValidator;
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

            var validationResult = _vacancyViewModelValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(VacancyMediatorCodes.GetVacancy.FailedValidation,
                    vacancyViewModel, validationResult);
            }

            if (vacancyViewModel == null)
            {
                return GetMediatorResponse<VacancyViewModel>(VacancyMediatorCodes.GetVacancy.NotAvailable);
            }

            return GetMediatorResponse(VacancyMediatorCodes.GetVacancy.Ok, vacancyViewModel);
        }
    }
}