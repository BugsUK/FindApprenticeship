namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using ViewModels.Vacancy;

    internal sealed class ApprenticeshipVacancyToVacancySummaryViewModelConverter : ITypeConverter<ApprenticeshipVacancy, VacancySummaryViewModel>
    {
        public VacancySummaryViewModel Convert(ResolutionContext context)
        {
            var source = (ApprenticeshipVacancy)context.SourceValue;

            var destination = new VacancySummaryViewModel
            {
                Duration = source.Duration.HasValue ? (decimal?) source.Duration.Value : null,
                DurationComment = source.DurationComment,
                DurationType = source.DurationType,
                HoursPerWeek = source.HoursPerWeek,
                LongDescription = source.LongDescription,
                LongDescriptionComment = source.LongDescriptionComment,
                VacancyReferenceNumber = source.VacancyReferenceNumber,
                Wage = source.Wage,
                WageComment = source.WageComment,
                WageType = source.WageType,
                WageUnit = source.WageUnit,
                WorkingWeek = source.WorkingWeek,
                WorkingWeekComment = source.WorkingWeekComment,
                Status = source.Status,
                VacancyDatesViewModel = context.Engine.Map<ApprenticeshipVacancy, VacancyDatesViewModel>(source)
            };


            return destination;
        }
    }
}