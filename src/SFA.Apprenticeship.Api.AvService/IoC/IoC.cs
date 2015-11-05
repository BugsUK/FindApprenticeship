namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using Apprenticeships.Infrastructure.Logging.IoC;
    using StructureMap;

    // NOTE: WCF IoC strategy is based on this article: https://lostechies.com/jimmybogard/2008/07/30/integrating-structuremap-with-wcf/.
    public static class IoC
    {
        static IoC()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
            });
        }

        public static IContainer Container { get; private set; }
    }
}
e