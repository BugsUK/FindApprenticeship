namespace SFA.DAS.RAA.Api.AcceptanceTests.Factories
{
    using System.Collections.Generic;
    using System.Data;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Domain.Entities.Raa.RaaApi;
    using Apprenticeships.Domain.Entities.Users;
    using Apprenticeships.Domain.Interfaces.Messaging;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Apprenticeships.Infrastructure.EmployerDataService.EmployerDataService;
    using Apprenticeships.Infrastructure.Postcode.Configuration;
    using Apprenticeships.Infrastructure.Repositories.Sql.Common;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.Entities;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.RaaApi;
    using Apprenticeships.Infrastructure.WebServices.Wcf;
    using Constants;
    using Infrastructure.Interfaces;
    using Moq;
    using UnitTests.Factories;

    public class RaaMockFactory
    {
        private static readonly Mock<IConfigurationService> MockConfigurationService = new Mock<IConfigurationService>();
        private static readonly Mock<IConfigurationManager> MockConfigurationManager = new Mock<IConfigurationManager>();
        private static readonly Mock<ILogService> MockLogService = new Mock<ILogService>();
        private static readonly Mock<IServiceBus> MockServiceBus = new Mock<IServiceBus>();
        private static readonly Mock<IVacancyReadRepository> MockVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private static readonly Mock<IProviderReadRepository> MockProviderReadRepository = new Mock<IProviderReadRepository>();
        private static readonly Mock<IProviderWriteRepository> MockProviderWriteRepository = new Mock<IProviderWriteRepository>();
        private static readonly Mock<IProviderSiteReadRepository> MockProviderSiteReadRepository = new Mock<IProviderSiteReadRepository>();
        private static readonly Mock<IProviderSiteWriteRepository> MockProviderSiteWriteRepository = new Mock<IProviderSiteWriteRepository>();
        private static readonly Mock<IVacancyOwnerRelationshipReadRepository> MockVacancyOwnerRelationshipReadRepository = new Mock<IVacancyOwnerRelationshipReadRepository>();
        private static readonly Mock<IVacancyOwnerRelationshipWriteRepository> MockVacancyOwnerRelationshipWriteRepository = new Mock<IVacancyOwnerRelationshipWriteRepository>();
        private static readonly Mock<IEmployerReadRepository> MockEmployerReadRepository = new Mock<IEmployerReadRepository>();
        private static readonly Mock<IEmployerWriteRepository> MockEmployerWriteRepository = new Mock<IEmployerWriteRepository>();
        private static readonly Mock<IGetOpenConnection> MockGetOpenConnection = new Mock<IGetOpenConnection>();
        private static readonly Mock<IDbConnection> MockDbConnection = new Mock<IDbConnection>();
        private static readonly Mock<IWcfService<EmployerLookupSoap>> MockEmployerLookupSoapService = new Mock<IWcfService<EmployerLookupSoap>>();

        static RaaMockFactory()
        {
            MockConfigurationService.Setup(cs => cs.Get<PostalAddressServiceConfiguration>()).Returns(new PostalAddressServiceConfiguration
            {
                RetrieveByIdEndpoint = "http://localhost",
                FindByPartsEndpoint = "http://localhost"
            });
            MockConfigurationService.Setup(cs => cs.Get<AddressConfiguration>()).Returns(new AddressConfiguration
            {
                FindServiceEndpoint = "http://localhost",
                RetrieveServiceEndpoint = "http://localhost"
            });
            
            //Setup mock for unkown user
            MockGetOpenConnection.Setup(m => m.Query<RaaApiUser>(It.IsAny<string>(), It.IsAny<object>(), null, null)).Returns(new List<RaaApiUser>());

            //Setup mocks for the two valid users
            var validProviderApiUser = RaaApiUserFactory.GetValidProviderApiUser(ApiKeys.ProviderApiKey);
            MockGetOpenConnection.Setup(
                m => m.Query<RaaApiUser>(RaaApiUserRepository.SelectSql, It.Is<object>(o => o.GetHashCode() == new { ApiKey = ApiKeys.ProviderApiKey }.GetHashCode()), null, null))
                .Returns(new [] {validProviderApiUser});

            MockGetOpenConnection.Setup(
                m => m.Query<RaaApiUser>(RaaApiUserRepository.SelectSql, It.Is<object>(o => o.GetHashCode() == new { ApiKey = ApiKeys.EmployerApiKey }.GetHashCode()), null, null))
                .Returns(new [] { RaaApiUserFactory.GetValidEmployerApiUser(ApiKeys.EmployerApiKey) });

            //Setup mock for unknown provider
            MockGetOpenConnection.Setup(m => m.Query<Provider>(It.IsAny<string>(), It.IsAny<object>(), null, null)).Returns(new List<Provider>());
            
            //Setup provider information for user
            MockGetOpenConnection.Setup(
                m => m.Query<Provider>(ProviderRepository.SelectByUkprnSql, It.Is<object>(o => o.GetHashCode() == new { ukprn = validProviderApiUser.ReferencedEntitySurrogateId.ToString(), providerStatusTypeID = ProviderStatuses.Activated }.GetHashCode()), null, null))
                .Returns(new[] { ProviderFactory.GetSkillsFundingAgencyProvider() });

            //Setup mock for unknown provider sites
            MockGetOpenConnection.Setup(m => m.Query<ProviderSite>(It.IsAny<string>(), It.IsAny<object>(), null, null)).Returns(new List<ProviderSite>());

            //Setup mock for unknown provider site relationships
            MockGetOpenConnection.Setup(m => m.Query<ProviderSiteRelationship>(It.IsAny<string>(), It.IsAny<object>(), null, null)).Returns(new List<ProviderSiteRelationship>());
        }

        public static Mock<IConfigurationService> GetMockConfigurationService() { return MockConfigurationService; }
        public static Mock<IConfigurationManager> GetMockConfigurationManager() { return MockConfigurationManager; }
        public static Mock<ILogService> GetMockLogService() { return MockLogService; }
        public static Mock<IServiceBus> GetMockServiceBus() { return MockServiceBus; }
        public static Mock<IVacancyReadRepository> GetMockVacancyReadRepository() { return MockVacancyReadRepository; }
        public static Mock<IProviderReadRepository> GetMockProviderReadRepository() { return MockProviderReadRepository; }
        public static Mock<IProviderWriteRepository> GetMockProviderWriteRepository() { return MockProviderWriteRepository; }
        public static Mock<IProviderSiteReadRepository> GetMockProviderSiteReadRepository() { return MockProviderSiteReadRepository; }
        public static Mock<IProviderSiteWriteRepository> GetMockProviderSiteWriteRepository() { return MockProviderSiteWriteRepository; }
        public static Mock<IVacancyOwnerRelationshipReadRepository> GetMockVacancyOwnerRelationshipReadRepository() { return MockVacancyOwnerRelationshipReadRepository; }
        public static Mock<IVacancyOwnerRelationshipWriteRepository> GetMockVacancyOwnerRelationshipWriteRepository() { return MockVacancyOwnerRelationshipWriteRepository; }
        public static Mock<IEmployerReadRepository> GetMockEmployerReadRepository() { return MockEmployerReadRepository; }
        public static Mock<IEmployerWriteRepository> GetMockEmployerWriteRepository() { return MockEmployerWriteRepository; }
        public static Mock<IGetOpenConnection> GetMockGetOpenConnection() { return MockGetOpenConnection; }
        public static Mock<IDbConnection> GetMockDbConnection() { return MockDbConnection; }
        public static Mock<IWcfService<EmployerLookupSoap>> GetMockEmployerLookupSoapService() { return MockEmployerLookupSoapService; }

    }
}