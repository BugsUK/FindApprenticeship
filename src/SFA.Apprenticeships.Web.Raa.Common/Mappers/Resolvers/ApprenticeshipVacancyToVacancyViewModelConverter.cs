namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;

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
            var destination = new VacancyViewModel
            {
                VacancyReferenceNumber = source.VacancyReferenceNumber,
                Status = source.Status,
                NewVacancyViewModel = context.Engine.Map<ApprenticeshipVacancy, NewVacancyViewModel>(source),
                VacancySummaryViewModel = context.Engine.Map<ApprenticeshipVacancy, VacancySummaryViewModel>(source),
                VacancyRequirementsProspectsViewModel =
                    context.Engine.Map<ApprenticeshipVacancy, VacancyRequirementsProspectsViewModel>(source),
                VacancyQuestionsViewModel = context.Engine.Map<ApprenticeshipVacancy, VacancyQuestionsViewModel>(source),
                LocationAddresses =
                    context.Engine.Map<List<VacancyLocationAddress>, List<VacancyLocationAddressViewModel>>(
                        source.LocationAddresses),
                IsEmployerLocationMainApprenticeshipLocation = source.IsEmployerLocationMainApprenticeshipLocation,
                NumberOfPositions = source.NumberOfPositions
            };

            return destination;
        }
    }
}
