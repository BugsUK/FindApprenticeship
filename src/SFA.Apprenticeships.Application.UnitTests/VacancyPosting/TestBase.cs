namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting
{
    using Domain.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories;
    using Interfaces.Providers;
    using Interfaces.VacancyPosting;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class TestBase
    {
        // Mocks.
        protected readonly Mock<IVacancyReadRepository> MockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        protected readonly Mock<IVacancyWriteRepository> MockApprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
        protected readonly Mock<IReferenceNumberRepository> MockReferenceNumberRepository = new Mock<IReferenceNumberRepository>();
        protected readonly Mock<IProviderUserReadRepository> MockProviderUserReadRepository = new Mock<IProviderUserReadRepository>();
        protected readonly Mock<IVacancyLocationReadRepository> MockVacancyLocationAddressReadRepository = new Mock<IVacancyLocationReadRepository>();
        protected readonly Mock<IVacancyLocationWriteRepository> MockVacancyLocationAddressWriteRepository = new Mock<IVacancyLocationWriteRepository>();
        protected readonly Mock<ICurrentUserService> MockCurrentUserService = new Mock<ICurrentUserService>();
        protected readonly Mock<IProviderVacancyAuthorisationService> MockProviderVacancyAuthorisationService = new Mock<IProviderVacancyAuthorisationService>();

        // Object under test.
        protected IVacancyPostingService VacancyPostingService;
    }
}
