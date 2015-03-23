using SFA.Apprenticeships.Infrastructure.Xml;

namespace SFA.Apprenticeships.Web.ContactForms.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Application.Services.Communication;
    using Application.Services.ConfigReferenceDataService;
    using Application.Services.LocationSearchService;
    using Common.AppSettings;
    using Domain.Entities;
    using Infrastructure.Communication.Email;
    using Infrastructure.Communication.Email.EmailMessageFormatters;
    using Infrastructure.Logging;
    using Mappers;
    using Mappers.Interfaces;
    using Mediators.AccessRequest;
    using Mediators.EmployerEnquiry;
    using Mediators.Interfaces;
    using Mediators.Location;
    using Mediators.ReferenceData;
    using Providers;
    using Providers.Interfaces;
    using StructureMap.Configuration.DSL;
    using ViewModels;
    using SFA.Apprenticeships.Application.Services.Communication.Strategies.Interfaces;
    using SFA.Apprenticeships.Application.Services.Communication.Strategies;

    public class ContactFormsWebRegistry : Registry
    {
        public ContactFormsWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<ILogService>().AlwaysUnique().Use<NLogLogService>().Ctor<Type>().Is(c => c.ParentType ?? c.RootType);

            RegisterStrategis();

            RegisterServices();

            RegisterMappers();

            RegisterProviders();

            RegisterMediators();
        }

        private void RegisterStrategis()
        {
            For<ISendAccessRequestStrategy>().Use<SendAccessRequestStrategy>().Ctor<IEmailDispatcher>().Named(BaseAppSettingValues.EmailDispatcher);
            For<ISendEmployerEnquiryStrategy>().Use<SendEmployerEnquiryStrategy>().Ctor<IEmailDispatcher>().Named(BaseAppSettingValues.EmailDispatcher);
            For<ISendGlaEmployerEnquiryStrategy>().Use<SendGlaEmployerEnquiryStrategy>().Ctor<IEmailDispatcher>().Named(BaseAppSettingValues.EmailDispatcher);
        }

        private void RegisterServices()
        {
            For<IXmlGenerator>().Use<XmlGenerator>();

            IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> emailMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.EmployerEnquiry, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.EmployerEnquiryConfirmation, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.GlaEmployerEnquiry, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.GlaEmployerEnquiryConfirmation, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.WebAccessRequest, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.WebAccessRequestConfirmation, new EmailSimpleMessageFormatter())
            };

            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Named("SendGridEmailDispatcher")
                .Ctor<IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>>>().Is(emailMessageFormatters);
            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            
            For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance);            
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ICommunciationService>().Use<CommunciationService>();
            For<IReferenceDataService>().Use<ConfigReferenceDataService>();
        }

        private void RegisterProviders()
        {
            For<IEmployerEnquiryProvider>().Use<EmployerEnquiryProvider>();
            For<IAccessRequestProvider>().Use<AccessRequestProvider>();
            For<ILocationProvider>().Use<LocationProvider>();
            For<IReferenceDataProvider>().Use<ReferenceDataProvider>();
        }

        private void RegisterMediators()
        {
            For<IEmployerEnquiryMediator>().Use<EmployerEnquiryMediator>();
            For<IAccessRequestMediator>().Use<AccessRequestMediator>();
            For<ILocationMediator>().Use<LocationMediator>();
            For<IReferenceDataMediator>().Use<ReferenceDataMediator>();
        }

        private void RegisterMappers()
        {
            For<IDomainToViewModelMapper<Address, AddressViewModel>>().Use<AddressMapper>();
            For<IViewModelToDomainMapper<AddressViewModel, Address>>().Use<AddressMapper>();

            For<IDomainToViewModelMapper<AccessRequest, AccessRequestViewModel>>().Use<AccessRequestMapper>();
            For<IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>>().Use<AccessRequestMapper>();

            For<IDomainToViewModelMapper<EmployerEnquiry, EmployerEnquiryViewModel>>().Use<EmployerEnquiryMapper>();
            For<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>>().Use<EmployerEnquiryMapper>();

            For<IDomainToViewModelMapper<Location, LocationViewModel>>().Use<LocationMapper>();
            For<IViewModelToDomainMapper<LocationViewModel, Location>>().Use<LocationMapper>();

            For<IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel>>().Use<ReferenceDataMapper>();
            For<IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData>>().Use<ReferenceDataMapper>();
        }
    }
}
