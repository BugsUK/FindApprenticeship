namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.Security;
    using System.ServiceModel;
    using Apprenticeships.Application.Interfaces.Logging;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
    using Namespaces.Version51;
    using Providers.Version51;
    using ServiceContracts.Version51;

    [ExceptionShielding("Default Exception Policy")]
    [ServiceBehavior(Namespace = Namespace.Uri)]
    public class VacancyDetailsService : IVacancyDetails
    {
        private readonly IVacancyDetailsProvider _vacancyDetailsProvider;

        public VacancyDetailsService(
            ILogService logService,
            IVacancyDetailsProvider vacancyDetailsProvider)
        {
            _vacancyDetailsProvider = vacancyDetailsProvider;
        }

        public VacancyDetailsResponse Get(VacancyDetailsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // TODO: API: AG: remove test code.
            if (request.MessageId == Guid.Empty)
            {
                throw new SecurityException();
            }

            throw new ArgumentNullException(nameof(request));

            // return _vacancyDetailsProvider.Get(request);
        }
    }
}
