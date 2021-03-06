namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    internal sealed class VacancyToFurtherVacancyDetailsViewModelConverter : ITypeConverter<Vacancy, FurtherVacancyDetailsViewModel>
    {
        public FurtherVacancyDetailsViewModel Convert(ResolutionContext context)
        {
            var source = (Vacancy)context.SourceValue;

            var destination = new FurtherVacancyDetailsViewModel
            {
                Duration = source.Duration.HasValue ? (decimal?) source.Duration.Value : null,
                DurationComment = source.DurationComment,
                DurationType = source.DurationType,
                ExpectedDuration = source.ExpectedDuration,
                LongDescription = source.LongDescription,
                LongDescriptionComment = source.LongDescriptionComment,
                VacancyReferenceNumber = source.VacancyReferenceNumber,
                Wage = context.Engine.Map<Wage, WageViewModel>(source.Wage),
                WageComment = source.WageComment,
                WorkingWeek = source.WorkingWeek,
                WorkingWeekComment = source.WorkingWeekComment,
                Status = source.Status,
                VacancyDatesViewModel = context.Engine.Map<Vacancy, VacancyDatesViewModel>(source),
                VacancyType = source.VacancyType
            };


            return destination;
        }
    }
}