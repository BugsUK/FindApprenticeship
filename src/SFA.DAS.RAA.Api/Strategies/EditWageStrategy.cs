namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Entities.Vacancies;
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

        public Vacancy EditWage(WageUpdate wage, int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            var vacancy = _vacancyProvider.Get(vacancyId, vacancyReferenceNumber, vacancyGuid);
            if (vacancy.VacancyType != VacancyType.Apprenticeship)
            {
                throw new ArgumentException("You can only edit the wage of an Apprenticeship vacancy.");
            }
            if (vacancy.Status != VacancyStatus.Live && vacancy.Status != VacancyStatus.Closed)
            {
                throw new ArgumentException("You can only edit the wage of a vacancy that is live or closed.");
            }
            if (!vacancy.Wage.HoursPerWeek.HasValue)
            {
                throw new ArgumentException("You can only edit the wage of a vacancy that has a valid value for hours per week.");
            }
            wage.ExistingType = vacancy.Wage.Type;
            wage.ExistingAmount = vacancy.Wage.Amount;
            wage.ExistingAmountLowerBound = vacancy.Wage.AmountLowerBound;
            wage.ExistingAmountUpperBound = vacancy.Wage.AmountUpperBound;
            wage.ExistingUnit = vacancy.Wage.Unit;

            var validator = new WageUpdateValidator();
            var validationResult = validator.Validate(wage, ruleSet: WageUpdateValidator.CompareWithExisting);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (wage.Type.HasValue)
            {
                vacancy.Wage.Type = wage.Type.Value;
            }

            if (vacancy.Wage.Type == WageType.LegacyWeekly || vacancy.Wage.Type == WageType.Custom)
            {
                vacancy.Wage.Amount = wage.Amount;
            }
            else
            {
                vacancy.Wage.Amount = null;
            }

            if (vacancy.Wage.Type == WageType.CustomRange)
            {
                vacancy.Wage.AmountLowerBound = wage.AmountLowerBound ?? wage.ExistingAmountLowerBound;
                vacancy.Wage.AmountUpperBound = wage.AmountUpperBound ?? wage.ExistingAmountUpperBound;
            }
            else
            {
                vacancy.Wage.AmountLowerBound = null;
                vacancy.Wage.AmountUpperBound = null;
            }

            if (wage.Unit.HasValue)
            {
                vacancy.Wage.Unit = wage.Unit.Value;
            }

            var updatedVacancy = _updateVacancyStrategy.UpdateVacancy(vacancy);

            return updatedVacancy;
        }
    }
}