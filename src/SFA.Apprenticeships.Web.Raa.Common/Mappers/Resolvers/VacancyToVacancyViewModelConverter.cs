namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using AutoMapper;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Presentation;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels.Locations;

    /// <summary>
    /// This is meant to replace
    /// SFA.Apprenticeships.Web.Raa.Common.Converters.ApprenticeshipVacancyConverter.ConvertToVacancyViewModel
    /// 
    /// TODO: DI for automapper within Raa websites (manage and recruit), then replace usage of converter (above) with this mapping class
    /// </summary>
    internal sealed class VacancyToVacancyViewModelConverter : ITypeConverter<Vacancy, VacancyViewModel>
    {
        public VacancyViewModel Convert(ResolutionContext context)
        {
            var source = (Vacancy)context.SourceValue;
            var destination = new VacancyViewModel
            {
                VacancyReferenceNumber = source.VacancyReferenceNumber,
                Status = source.Status,
                NewVacancyViewModel = context.Engine.Map<Vacancy, NewVacancyViewModel>(source),
                TrainingDetailsViewModel = context.Engine.Map<Vacancy, TrainingDetailsViewModel>(source),
                FurtherVacancyDetailsViewModel = context.Engine.Map<Vacancy, FurtherVacancyDetailsViewModel>(source),
                VacancyRequirementsProspectsViewModel = context.Engine.Map<Vacancy, VacancyRequirementsProspectsViewModel>(source),
                VacancyQuestionsViewModel = context.Engine.Map<Vacancy, VacancyQuestionsViewModel>(source),
                OfflineApplicationClickThroughCount = source.OfflineApplicationClickThroughCount,
                VacancyType = source.VacancyType,
                Address = context.Engine.Map<PostalAddress, AddressViewModel>(source.Address),
                VacancySource = source.VacancySource
            };

            if (source.Status.IsStateInQa())
            {
                destination.ContactDetailsAndVacancyHistory = new ContactDetailsAndVacancyHistoryViewModel
                {
                    DateSubmitted = source.DateSubmitted,
                    DateFirstSubmitted = source.DateFirstSubmitted ?? source.DateSubmitted,
                    DateLastUpdated = source.UpdatedDateTime
                };
            }

            // TODO: move to its custom mapper?
            destination.NewVacancyViewModel.VacancyGuid = source.VacancyGuid;
            //TODO: Map after this conversion
            //destination.NewVacancyViewModel.LocationAddresses =
            //    context.Engine.Map<List<VacancyLocation>, List<VacancyLocationAddressViewModel>>(
            //        source.LocationAddresses);
                
            destination.NewVacancyViewModel.AdditionalLocationInformation = source.AdditionalLocationInformation;
            destination.NewVacancyViewModel.IsEmployerLocationMainApprenticeshipLocation =
                source.IsEmployerLocationMainApprenticeshipLocation;
            destination.NewVacancyViewModel.NumberOfPositions = source.NumberOfPositions;
            destination.NewVacancyViewModel.NumberOfPositionsComment = source.NumberOfPositionsComment;

            return destination;
        }
    }
}
