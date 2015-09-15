using Moq;
using SFA.Apprenticeships.Web.Recruit.Mediators.Home;
using SFA.Apprenticeships.Web.Recruit.Providers;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Home
{
    public class HomeMediatorBuilder
    {
        private Mock<IProviderProvider> _providerProvider = new Mock<IProviderProvider>();

        public IHomeMediator Build()
        {
            var mediator = new HomeMediator(_providerProvider.Object);
            return mediator;
        }

        public HomeMediatorBuilder With(Mock<IProviderProvider> providerProvider)
        {
            _providerProvider = providerProvider;
            return this;
        }
    }
}