namespace SFA.DAS.RAA.Api.AcceptanceTests.DependencyResolution
{
    using Apprenticeships.Application.Employer.Strategies;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Domain.Interfaces.Messaging;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Apprenticeships.Infrastructure.Common.CurrentUser;
    using Apprenticeships.Infrastructure.Common.DateTime;
    using Apprenticeships.Infrastructure.EmployerDataService.EmployerDataService;
    using Apprenticeships.Infrastructure.Postcode.Configuration;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo;
    using Apprenticeships.Infrastructure.WebServices.Wcf;
    using Infrastructure.Interfaces;
    using Moq;
    using StructureMap.Configuration.DSL;

    public class AcceptanceTestsRegistry : Registry
    {
        public AcceptanceTestsRegistry()
        {
            //Mocks for all tests
            var mockConfigurationService = new Mock<IConfigurationService>();
            mockConfigurationService.Setup(cs => cs.Get<PostalAddressServiceConfiguration>()).Returns(new PostalAddressServiceConfiguration
            {
                RetrieveByIdEndpoint = "http://localhost",
                FindByPartsEndpoint = "http://localhost"
            });
            mockConfigurationService.Setup(cs => cs.Get<AddressConfiguration>()).Returns(new AddressConfiguration
            {
                FindServiceEndpoint = "http://localhost",
                RetrieveServiceEndpoint = "http://localhost"
            });
            For<IConfigurationService>().Use(mockConfigurationService.Object);
            For<IConfigurationManager>().Use(new Mock<IConfigurationManager>().Object);
            For<ILogService>().Use(new Mock<ILogService>().Object);
            For<IServiceBus>().Use(new Mock<IServiceBus>().Object);

            For<IVacancyReadRepository>().Use(new Mock<IVacancyReadRepository>().Object);
            For<IProviderReadRepository>().Use(new Mock<IProviderReadRepository>().Object);
            For<IProviderWriteRepository>().Use(new Mock<IProviderWriteRepository>().Object);
            For<IProviderSiteReadRepository>().Use(new Mock<IProviderSiteReadRepository>().Object);
            For<IProviderSiteWriteRepository>().Use(new Mock<IProviderSiteWriteRepository>().Object);
            For<IVacancyOwnerRelationshipReadRepository>().Use(new Mock<IVacancyOwnerRelationshipReadRepository>().Object);
            For<IVacancyOwnerRelationshipWriteRepository>().Use(new Mock<IVacancyOwnerRelationshipWriteRepository>().Object);
            For<IEmployerReadRepository>().Use(new Mock<IEmployerReadRepository>().Object);
            For<IEmployerWriteRepository>().Use(new Mock<IEmployerWriteRepository>().Object);

            //Mocked as we don't want real calls to go to EDRS
            For<IWcfService<EmployerLookupSoap>>().Use(new Mock<IWcfService<EmployerLookupSoap>>().Object);

            //Normally in the CommonRegistry but need to do manually in acceptance tests due to test version of ConfigurationManager and Service
            For<IDateTimeService>().Use<DateTimeService>();
            For<ICurrentUserService>().Use(new Mock<CurrentUserService>().Object);

            //TODO: Should be registered in the service the strategy belongs to
            For<IMapper>().Singleton().Use<EmployerMappers>().Name = "EmployerMappers";
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
        }
    }
}