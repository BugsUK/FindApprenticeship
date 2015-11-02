namespace SFA.Apprenticeship.Api.AvmsCompatability.IoC
{
    using Apprenticeships.Domain.Interfaces.Mapping;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Mappers.Version51;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();

                x.For<IMapper>().Singleton().Use<AvReferenceDataServiceMapper>();
            });

            return container;
        }
    }
}
