namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Models;
    using Providers;
    using Validators;
    using FluentValidation;

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
            wage.ExistingAmount = vacancy.Wage.Amount;

            //TODO: Loads of validation
            var validator = new WageUpdateValidator();
            var validationResult = validator.Validate(wage, ruleSet: WageUpdateValidator.CompareWithExisting);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            vacancy.Wage.Type = wage.Type;
            vacancy.Wage.Amount = wage.Amount;
            vacancy.Wage.AmountLowerBound = wage.AmountLowerBound;
            vacancy.Wage.AmountUpperBound = wage.AmountUpperBound;
            vacancy.Wage.Unit = wage.Unit;

            var updatedVacancy = _updateVacancyStrategy.UpdateVacancy(vacancy);

            return updatedVacancy;
        }
    }
}