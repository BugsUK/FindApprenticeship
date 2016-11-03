namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.Raa.Vacancies;
    using Mappers;
    using ViewModels.Admin;

    public class StandardsAndFrameworksProvider : IStandardsAndFrameworksProvider
    {
        private static readonly IMapper StandardMappers = new StandardMappers();

        private readonly IReferenceDataService _referenceDataService;

        public StandardsAndFrameworksProvider(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        public StandardViewModel CreateStandard(StandardViewModel viewModel)
        {
            var standard = StandardMappers.Map<StandardViewModel, Standard>(viewModel);

            var createdStandard = _referenceDataService.CreateStandard(standard);

            return StandardMappers.Map<Standard, StandardViewModel>(createdStandard);
        }
    }
}