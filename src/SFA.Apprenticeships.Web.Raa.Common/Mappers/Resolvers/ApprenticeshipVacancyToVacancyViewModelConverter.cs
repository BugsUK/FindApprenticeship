namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using ViewModels.Vacancy;

    /// <summary>
    /// This is meant to replace
    /// SFA.Apprenticeships.Web.Raa.Common.Converters.ApprenticeshipVacancyConverter.ConvertToVacancyViewModel
    /// 
    /// TODO: DI for automapper within Raa websites (manage and recruit), then replace usage of converter (above) with this mapping class
    /// </summary>
    internal sealed class ApprenticeshipVacancyToVacancyViewModelConverter : ITypeConverter<ApprenticeshipVacancy, VacancyViewModel>
    {
        public VacancyViewModel Convert(ResolutionContext context)
        {
            var source = (ApprenticeshipVacancy)context.SourceValue;
            var destination = new VacancyViewModel();
            destination.VacancyReferenceNumber = source.VacancyReferenceNumber;
            destination.Status= source.Status;
            destination.NewVacancyViewModel = context.Engine.Map<ApprenticeshipVacancy, NewVacancyViewModel>(source);
            destination.VacancySummaryViewModel = context.Engine.Map<ApprenticeshipVacancy, VacancySummaryViewModel>(source);
            destination.VacancyRequirementsProspectsViewModel = context.Engine.Map<ApprenticeshipVacancy, VacancyRequirementsProspectsViewModel>(source);
            destination.VacancyQuestionsViewModel = context.Engine.Map<ApprenticeshipVacancy, VacancyQuestionsViewModel>(source);

            return destination;
        }
    }
}
