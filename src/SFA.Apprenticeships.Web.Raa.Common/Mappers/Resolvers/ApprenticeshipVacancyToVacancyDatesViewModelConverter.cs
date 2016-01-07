namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System;
    using AutoMapper;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    public class ApprenticeshipVacancyToVacancyDatesViewModelConverter :
        ITypeConverter<ApprenticeshipVacancy, VacancyDatesViewModel>
    {
        public VacancyDatesViewModel Convert(ResolutionContext context)
        {
            var source = (ApprenticeshipVacancy)context.SourceValue;

            var destination = new VacancyDatesViewModel
            {
                ClosingDate = context.Engine.Map<DateTime?, DateViewModel>(source.ClosingDate),
                PossibleStartDate = context.Engine.Map<DateTime?, DateViewModel>(source.PossibleStartDate),
                VacancyReferenceNumber = source.VacancyReferenceNumber
            };


            return destination;
        }
    }
}