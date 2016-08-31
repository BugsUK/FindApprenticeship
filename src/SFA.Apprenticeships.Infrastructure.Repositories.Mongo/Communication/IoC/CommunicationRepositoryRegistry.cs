namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication.IoC
{
    using Domain.Interfaces.Repositories;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class CommunicationRepositoryRegistry : Registry
    {
        public CommunicationRepositoryRegistry()
        {
            For<IMapper>().Use<CommunicationMappers>().Name = "CommunicationMappers";
            
            For<IExpiringApprenticeshipApplicationDraftRepository>()
                .Use<ExpiringApprenticeshipApplicationDraftRepository>()
                .Ctor<IMapper>()
                .Named("CommunicationMappers");

            For<IApplicationStatusAlertRepository>()
                .Use<ApplicationStatusAlertRepository>()
                .Ctor<IMapper>()
                .Named("CommunicationMappers");

            For<ISavedSearchAlertRepository>()
                .Use<SavedSearchAlertRepository>()
                .Ctor<IMapper>()
                .Named("CommunicationMappers");

            For<IContactMessageRepository>()
                .Use<ContactMessageRepository>()
                .Ctor<IMapper>()
                .Named("CommunicationMappers");
        }
    }
}
