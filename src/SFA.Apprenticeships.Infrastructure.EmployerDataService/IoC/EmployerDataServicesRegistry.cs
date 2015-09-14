namespace SFA.Apprenticeships.Infrastructure.EmployerDataService.IoC
{
    using Application.Organisation;
    using EmployerDataService;
    using Providers;
    using StructureMap.Configuration.DSL;
    using WebServices.Wcf;

    public class EmployerDataServicesRegistry : Registry
    {
        public EmployerDataServicesRegistry()
        {
            // Providers.
            For<IVerifiedOrganisationProvider>().Use<EmployerDataProvider>().Singleton();

            // Web Services.
            For<IWcfService<EmployerLookupSoapClient>>().Use<WcfService<EmployerLookupSoapClient>>();
        }
    }
}
