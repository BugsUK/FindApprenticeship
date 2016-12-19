namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
    using Constants;
    using Models;
    using Providers;
    using Validators;
    using FluentValidation;
    using VacancyType = Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType;

    public class EditWageStrategy : IEditWageStrategy
    {
        private readonly IVacancyProvider _vacancyProvider;
        private readonly IUpdateVacancyStrategy _updateVacancyStrategy;

        public EditWageStrategy(IVacancyProvider vacancyProvider, IUpdateVacancyStrategy updateVacancyStrategy)
        {
            _vacancyProvider = vacancyProvider;
            _updateVacancyStrategy = updateVacancyStrategy;
        }

        public Vacancy EditWage(WageUpdate wageUpdate, int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            var vacancy = _vacancyProvider.Get(vacancyId, vacancyReferenceNumber, vacancyGuid);
            if (vacancy.VacancyType != VacancyType.Apprenticeship)
            {
                throw new ArgumentException(WageUpdateMessages.CannotEditTraineeshipWage);
            }
            if (vacancy.Status != VacancyStatus.Live && vacancy.Status != VacancyStatus.Closed)
            {
                throw new ArgumentException(WageUpdateMessages.CannotEditNonLiveWage);
            }
            if (!vacancy.Wage.HoursPerWeek.HasValue)
            {
                throw new ArgumentException(WageUpdateMessages.MissingHoursPerWeek);
            }
            wageUpdate.ExistingWage = vacancy.Wage;
            wageUpdate.PossibleStartDate = vacancy.PossibleStartDate;

            var validator = new WageUpdateValidator();
            var validationResult = validator.Validate(wageUpdate, ruleSet: WageUpdateValidator.CompareWithExisting);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (wageUpdate.Type.HasValue)
            {
                vacancy.Wage.Type = wageUpdate.Type.Value;
            }

            if (vacancy.Wage.Type == WageType.LegacyWeekly || vacancy.Wage.Type == WageType.Custom)
            {
                vacancy.Wage.Amount = wageUpdate.Amount;
            }
            else
            {
                vacancy.Wage.Amount = null;
            }

            if (vacancy.Wage.Type == WageType.CustomRange)
            {
                vacancy.Wage.AmountLowerBound = wageUpdate.AmountLowerBound ?? wageUpdate.ExistingWage.AmountLowerBound;
                vacancy.Wage.AmountUpperBound = wageUpdate.AmountUpperBound ?? wageUpdate.ExistingWage.AmountUpperBound;
            }
            else
            {
                vacancy.Wage.AmountLowerBound = null;
                vacancy.Wage.AmountUpperBound = null;
            }

            if (wageUpdate.Unit.HasValue)
            {
                vacancy.Wage.Unit = wageUpdate.Unit.Value;
            }

            var updatedVacancy = _updateVacancyStrategy.UpdateVacancy(vacancy);

            return updatedVacancy;
        }
    }
}