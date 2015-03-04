﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication.IoC
{
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class CommunicationRepositoryRegistry : Registry
    {
        public CommunicationRepositoryRegistry()
        {
            For<IMapper>().Use<CommunicationMappers>().Name = "CommunicationMappers";
            For<IExpiringApprenticeshipApplicationDraftRepository>()
                .Use<ExpiringApprenticeshipApplicationDraftRepository>()
                .Ctor<CommunicationMappers>()
                .Named("CommunicationMappers");
            For<IApplicationStatusAlertRepository>()
                .Use<ApplicationStatusAlertRepository>()
                .Ctor<CommunicationMappers>()
                .Named("CommunicationMappers");
        }
    }
}
