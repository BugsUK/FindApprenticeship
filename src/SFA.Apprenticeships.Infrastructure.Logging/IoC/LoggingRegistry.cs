namespace SFA.Apprenticeships.Infrastructure.Logging.IoC
{
    using System;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class LoggingRegistry : Registry
    {
        public LoggingRegistry()
        {
            For<ILogService>().AlwaysUnique().Use<NLogLogService>().Ctor<Type>().Is(c => c.ParentType ?? c.RootType);
        }
    }
}
