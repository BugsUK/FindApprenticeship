﻿namespace SFA.DAS.RAA.Api.Strategies
{
    using System;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Models;
    using Providers;
    using Validators;
    using FluentValidation;

    public class EditWageStrategy : IEditWageStrategy
    {
        private readonly IVacancyProvider _vacancyProvider;

        public EditWageStrategy(IVacancyProvider vacancyProvider)
        {
            _vacancyProvider = vacancyProvider;
        }

        public Vacancy EditWage(WageUpdate wage, int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            var vacancy = _vacancyProvider.Get(vacancyId, vacancyReferenceNumber, vacancyGuid);
            if (vacancy.Status != VacancyStatus.Live && vacancy.Status != VacancyStatus.Closed)
            {
                throw new ArgumentException("You can only edit the wage of a vacancy that is live or closed.");
            }
            wage.ExistingAmount = vacancy.Wage.Amount;

            //TODO: Loads of validation
            var validator = new WageUpdateValidator();
            var validationResult = validator.Validate(wage, ruleSet: "CompareWithExisting");
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            vacancy.Wage.Amount = wage.Amount;
            vacancy.Wage.Unit = wage.Unit;

            return vacancy;
        }
    }
}