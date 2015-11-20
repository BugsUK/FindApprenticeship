namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using DataContracts.Version51;
    using MessageContracts.Version51;

    // REF: NAVMS: Navms.Ms.ExternalInterfaces.ServiceImplementation.Rel51.VacancyManagementInternalService
    // REF: NAVMS: Capgemini.LSC.Navms.MS.BusinessLogic.VacancyController::BulkUploadVacancies

    public class VacancyUploadProvider : IVacancyUploadProvider
    {
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyUploadProvider(IVacancyPostingService vacancyPostingService)
        {
            _vacancyPostingService = vacancyPostingService;
        }

        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var vacancies = new List<VacancyUploadResultData>();

            if (request.Vacancies?.Count > 0)
            {
                vacancies.AddRange(request.Vacancies
                    .Select(vacancyUploadData => UploadVacancy()));
            }

            return new VacancyUploadResponse
            {
                MessageId = request.MessageId,
                Vacancies = vacancies
            };
        }

        #region Helpers

        private VacancyUploadResultData UploadVacancy()
        {
            _vacancyPostingService.SaveApprenticeshipVacancy(new ApprenticeshipVacancy());

            var vacancyUploadResultData = new VacancyUploadResultData
            {
                ErrorCodes = new List<ElementErrorData>()
            };

            return vacancyUploadResultData;
        }

        #endregion
    }
}
