namespace SFA.Apprenticeship.Api.AvmsCompatability.IoC
{
    using Apprenticeships.Infrastructure.Logging.IoC;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
            });
        }
    }
}
