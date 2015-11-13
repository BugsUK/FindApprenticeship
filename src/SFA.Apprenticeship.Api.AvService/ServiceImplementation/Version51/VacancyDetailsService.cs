namespace SFA.Apprenticeship.Api.AvService.ServiceImplementation.Version51
{
    using System;
    using System.Security;
    using System.ServiceModel;
    using Apprenticeships.Application.Interfaces.Logging;
    using MessageContracts.Version51;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
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
            var type = typeof(Exception);
            var type2 = typeof (Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlingSettings);

            if (request.MessageId.ToString() == Guid.Empty.ToString())
            {
                try
                {
                    throw new SecurityException("Something wonderful happened.");
                }
                catch (Exception e)
                {
                    var rethrow = ExceptionPolicy.HandleException(e, "Default Exception Policy");

                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
 
            return _vacancyDetailsProvider.Get(request);
        }
    }
}
 