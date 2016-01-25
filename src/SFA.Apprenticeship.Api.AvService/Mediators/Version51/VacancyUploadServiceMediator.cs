namespace SFA.Apprenticeship.Api.AvService.Mediators.Version51
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Interfaces.VacancyPosting;
    using Common;
    using DataContracts.Version51;
    using Domain;
    using Mappers.Version51;
    using MessageContracts.Version51;
    using Providers;
    using Validators;
    using Validators.Version51;

    // REF: NAVMS: Navms.Ms.ExternalInterfaces.ServiceImplementation.Rel51.VacancyManagementInternalService
    // REF: NAVMS: Capgemini.LSC.Navms.MS.BusinessLogic.VacancyController::BulkUploadVacancies

    public class VacancyUploadServiceMediator : ServiceMediatorBase, IVacancyUploadServiceMediator
    {
        private readonly VacancyUploadDataValidator _validator;
        private readonly IVacancyUploadRequestMapper _vacancyUploadRequestMapper;

        private readonly IProviderService _providerService;
        private readonly IVacancyPostingService _vacancyPostingService;

        public VacancyUploadServiceMediator(
            VacancyUploadDataValidator validator,
            IVacancyUploadRequestMapper vacancyUploadRequestMapper,
            IWebServiceAuthenticationProvider webServiceAuthenticationProvider,
            IProviderService providerService,
            IVacancyPostingService vacancyPostingService) :
            base(webServiceAuthenticationProvider, WebServiceCategory.VacancyUpload)
        {
            _validator = validator;
            _vacancyUploadRequestMapper = vacancyUploadRequestMapper;
            _providerService = providerService;
            _vacancyPostingService = vacancyPostingService;
        }

        public VacancyUploadResponse UploadVacancies(VacancyUploadRequest request)
        {
            AuthenticateRequest(request);

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

            var providerSiteErn = Convert.ToString(vacancyUploadData.VacancyOwnerEdsUrn);
            var ern = Convert.ToString(vacancyUploadData.Employer?.EdsUrn);

            var providerSiteEmployerLink = _providerService.GetProviderSiteEmployerLink(providerSiteErn, ern);

            var vacancyReferenceNumber = _vacancyPostingService.GetNextVacancyReferenceNumber();
            var vacancy = _vacancyUploadRequestMapper.ToVacancy(vacancyReferenceNumber, vacancyUploadData, providerSiteEmployerLink);

            var newVacancy = _vacancyPostingService.CreateApprenticeshipVacancy(vacancy);

            var vacancyUploadResultData = new VacancyUploadResultData
            {
                VacancyId = vacancyUploadData.VacancyId,
                ReferenceNumber = Convert.ToInt32(newVacancy.VacancyReferenceNumber),
                Status = VacancyUploadResult.Success,
                ErrorCodes = new List<ElementErrorData>()
            };

            return vacancyUploadResultData;
        }

        #endregion
    }
}
