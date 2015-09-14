using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Home
{
    public class HomeMediatorBuilder
    {
        public IHomeMediator Build()
        {
            var mediator = new HomeMediator();
            return mediator;
        }
    }
}