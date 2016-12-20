namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies;
    using ApiVacancy = DAS.RAA.Api.Client.V1.Models.Vacancy;
    using Wage = Domain.Entities.Vacancies.Wage;

    public class ApiWageToWageResolver : ValueResolver<ApiVacancy, Wage>
    {
        protected override Wage ResolveCore(ApiVacancy source)
        {
            var apiWage = source.Wage;
            var type = (WageType)Enum.Parse(typeof(WageType), apiWage.Type);
            var unit = (WageUnit)Enum.Parse(typeof(WageUnit), apiWage.Unit);

            return new Wage(type, apiWage.Amount, apiWage.AmountLowerBound, apiWage.AmountUpperBound, apiWage.Text, unit, apiWage.HoursPerWeek, apiWage.ReasonForType);
        }
    }
}