namespace SFA.Apprenticeships.Web.ContactForms.Ioc
{
    using System;
    using System.Web;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Application.Services.CommunicationService;
    using Application.Services.ConfigReferenceDataService;
    using Application.Services.LocationSearchService;
    using Common.AppSettings;
    using Domain.Entities;
    using Infrastructure.Communication.Email;
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

    public class ContactFormsWebRegistry : Registry
    {
        public ContactFormsWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            RegisterServices();

            RegisterMappers();

            RegisterProviders();

            RegisterMediators();
        }

        private void RegisterServices()
        {
            For<ILogService>().AlwaysUnique().Use<NLogLogService>().Ctor<Type>().Is(c => c.ParentType ?? c.RootType);
            For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Name = "SendGridEmailDispatcher";
            For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ICommunciationService>().Use<CommunciationService>().Ctor<IEmailDispatcher>().Named(BaseAppSettingValues.EmailDispatcher);
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
