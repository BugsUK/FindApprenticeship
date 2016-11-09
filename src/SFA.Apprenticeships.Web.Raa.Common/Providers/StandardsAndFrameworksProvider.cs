﻿namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces;
    using Application.Interfaces.ReferenceData;
    using Converters;
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

        public StandardViewModel GetStandardViewModel(int standardId)
        {
            var standard = _referenceDataService.GetStandard(standardId);

            return standard.Convert();
        }

        public StandardViewModel SaveStandard(StandardViewModel viewModel)
        {
            var standard = _referenceDataService.GetStandard(viewModel.StandardId);

            //Copy over changes
            standard.Name = viewModel.Name;
            standard.ApprenticeshipSectorId = viewModel.ApprenticeshipSectorId;
            standard.ApprenticeshipLevel = viewModel.ApprenticeshipLevel;

            var updatedStandard = _referenceDataService.SaveStandard(standard);

            return StandardMappers.Map<Standard, StandardViewModel>(updatedStandard);
        }
    }
}