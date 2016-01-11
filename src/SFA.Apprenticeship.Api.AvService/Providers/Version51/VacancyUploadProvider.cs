namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Builders.Version51;
    using Common;
    using DataContracts.Version51;
    using MessageContracts.Version51;
    using Validators;

    // REF: NAVMS: Navms.Ms.ExternalInterfaces.ServiceImplementation.Rel51.VacancyManagementInternalService
    // REF: NAVMS: Capgemini.LSC.Navms.MS.BusinessLogic.VacancyController::BulkUploadVacancies

    public class VacancyUploadProvider : IVacancyUploadProvider
    {
        private readonly VacancyUploadDataValidator _validator;
        private readonly ApprenticeshipVacancyBuilder _apprenticeshipVacancyBuilder;
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyUploadProvider(
            VacancyUploadDataValidator validator,
            ApprenticeshipVacancyBuilder apprenticeshipVacancyBuilder,
            IVacancyPostingService vacancyPostingService)
        {
            _validator = validator;
            _apprenticeshipVacancyBuilder = apprenticeshipVacancyBuilder;
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
                vacancies
                    .AddRange(request.Vacancies
                        .Select(UploadVacancy));
            }

            return new VacancyUploadResponse
            {
                MessageId = request.MessageId,
                Vacancies = vacancies
            };
        }

        #region Helpers

        private VacancyUploadResultData UploadVacancy(VacancyUploadData vacancyUploadData)
        {
            var validationResult = _validator.Validate(vacancyUploadData);

            if (!validationResult.IsValid)
            {
                var errorCodes = validationResult
                    .GetErrorCodes()
                    .Select(each => new ElementErrorData
                    {
                        ErrorCode = each.InterfaceErrorCode
                    })
                    .ToList();

                return new VacancyUploadResultData
                {
                    VacancyId = vacancyUploadData.VacancyId,
                    Status = VacancyUploadResult.Failure,
                    ErrorCodes = errorCodes
                };
            }

            var mappedVacancy = _apprenticeshipVacancyBuilder.ToApprenticeshipVacancy(vacancyUploadData);
            var createdVacancy = _vacancyPostingService.CreateApprenticeshipVacancy(mappedVacancy);

            var vacancyUploadResultData = new VacancyUploadResultData
            {
                VacancyId = vacancyUploadData.VacancyId,
                ReferenceNumber = Convert.ToInt32(createdVacancy.VacancyReferenceNumber),
                Status = VacancyUploadResult.Success,
                ErrorCodes = null
            };

            return vacancyUploadResultData;
        }

        #endregion
    }
}
