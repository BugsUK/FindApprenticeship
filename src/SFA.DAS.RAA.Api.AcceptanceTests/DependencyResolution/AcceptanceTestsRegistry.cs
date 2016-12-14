namespace SFA.DAS.RAA.Api.AcceptanceTests.DependencyResolution
{
    using Apprenticeships.Application.Employer.Strategies;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Domain.Interfaces.Messaging;
    using Apprenticeships.Infrastructure.Common.CurrentUser;
    using Apprenticeships.Infrastructure.Common.DateTime;
    using Apprenticeships.Infrastructure.EmployerDataService.EmployerDataService;
    using Apprenticeships.Infrastructure.Repositories.Sql.Common;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo;
    using Apprenticeships.Infrastructure.WebServices.Wcf;
    using Factories;
    using Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class AcceptanceTestsRegistry : Registry
    {
        public AcceptanceTestsRegistry()
        {
            //Mocks for all tests
            For<IConfigurationService>().Use(RaaMockFactory.GetMockConfigurationService().Object);
            For<IConfigurationManager>().Use(RaaMockFactory.GetMockConfigurationManager().Object);
            For<ILogService>().Use(RaaMockFactory.GetMockLogService().Object);
            For<IServiceBus>().Use(RaaMockFactory.GetMockServiceBus().Object);

            For<IGetOpenConnection>().Use(RaaMockFactory.GetMockGetOpenConnection().Object).Name = "ReportingConnectionString";
            For<IGetOpenConnection>().Use(RaaMockFactory.GetMockGetOpenConnection().Object);

            //Mocked as we don't want real calls to go to EDRS
            For<IWcfService<EmployerLookupSoap>>().Use(RaaMockFactory.GetMockEmployerLookupSoapService().Object);

            //Normally in the CommonRegistry but need to do manually in acceptance tests due to test version of ConfigurationManager and Service
            For<IDateTimeService>().Use<DateTimeService>();
            For<ICurrentUserService>().Use<CurrentUserService>();

            //TODO: Should be registered in the service the strategy belongs to
            For<IMapper>().Singleton().Use<EmployerMappers>().Name = "EmployerMappers";
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
        }
    }
}