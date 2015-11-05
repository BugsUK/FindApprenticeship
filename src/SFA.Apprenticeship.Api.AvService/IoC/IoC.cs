namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using Apprenticeships.Infrastructure.Logging.IoC;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
            });

            return container;
        }
    }
}
