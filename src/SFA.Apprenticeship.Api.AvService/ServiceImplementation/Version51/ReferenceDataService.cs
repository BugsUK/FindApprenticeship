namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.ServiceModel;
    using Infrastructure.Interfaces;
    using Mediators.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using MessageContracts.Version51;
    using Namespaces.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class ReferenceDataService : IReferenceData
    {
        private readonly ILogService _logService;
        private readonly IReferenceDataServiceMediator _mediator;

        public ReferenceDataService(
            ILogService logService,
            IReferenceDataServiceMediator mediator)
        {
            _logService = logService;
            _mediator = mediator;
        }

        public GetErrorCodesResponse GetErrorCodes(GetErrorCodesRequest request)
        {
            object context = new
            {
                request?.ExternalSystemId,
                request?.MessageId
            };

            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                return _mediator.GetErrorCodes(request);
            }
            catch (Exception e)
            {
                _logService.Error(e, context);
                throw;
            }
        }

        public GetApprenticeshipFrameworksResponse GetApprenticeshipFrameworks(GetApprenticeshipFrameworksRequest request)
        {
            throw new NotImplementedException();
        }

        public GetRegionResponse GetRegion(GetRegionRequest request)
        {
            throw new NotImplementedException();
        }

        public GetCountiesResponse GetCounties(GetCountiesRequest request)
        {
            throw new NotImplementedException();
        }

        public GetLocalAuthoritiesResponse GetLocalAuthorities(GetLocalAuthoritiesRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
