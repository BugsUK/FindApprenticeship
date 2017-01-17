namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Common.Binders;
    using Common.Configuration;
    using Common.Framework;
    using Common.IoC;
    using Common.Validations;
    using Configuration;
    using Controllers;
    using SFA.Infrastructure.Interfaces;
    using FluentValidation.Mvc;
    using FluentValidation.Validators;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging;
    using Infrastructure.Logging.IoC;

    using SFA.Apprenticeships.Application.Interfaces;

    using StructureMap;

    public class MvcApplication : HttpApplication
    {

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            bool isWebsiteOffline = ConfigurationService.Get<CommonWebConfiguration>().IsWebsiteOffline;

            var context = HttpContext.Current;
            var path = context.Request.Path.ToLower();

            var lastExtension = path.LastIndexOf(".aspx", StringComparison.Ordinal);
            if (lastExtension != -1)
            {
                return;
            }
            var lastContent = path.LastIndexOf("/content/", StringComparison.Ordinal);
            if (lastContent != -1)
            {
                return;
            }

            if (isWebsiteOffline)
            {               
                context.RewritePath("~/403.aspx", false);
            }
        }

        private readonly static object _lock = new object();
        private static IConfigurationService _configurationService;
        private IConfigurationService ConfigurationService
        {
            get
            {
                if (_configurationService != null)
                {
                    return _configurationService;
                }

                lock (_lock)
                {
                    if (_configurationService == null)
                    {
                        var uncachedContiner = new Container(x =>
                        {
                            x.AddRegistry<CommonRegistry>();
                            x.AddRegistry<LoggingRegistry>();
                        });

                        var configurationService = uncachedContiner.GetInstance<IConfigurationService>();
                        var cacheConfig = configurationService.Get<CacheConfiguration>();

                        var cachedContiner = new Container(x =>
                        {
                            x.AddRegistry<LoggingRegistry>();
                            x.AddRegistry(new CommonRegistry(cacheConfig));
                            x.AddCachingRegistry(cacheConfig);
                        });

                        _configurationService = cachedContiner.GetInstance<IConfigurationService>();
                    }
                }

                return _configurationService;
            }
        }

        protected void Application_Start()
        {
            VersionLogging.SetVersion();
            RuntimeHelper.SetRuntimeName("SFA.Apprenticeships.Web.Candidate");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var container = (IContainer) DependencyResolver.Current.GetService(typeof(IContainer));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, container);
            RouteConfig.RegisterRoutes(RouteTable.Routes, RouteConfigOptions.MapMvcAttributeRoutes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory());

            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.AddImplicitRequiredValidator = false;
                provider.Add(typeof (EqualValidator),
                    (metadata, context, description, validator) =>
                        new EqualToValueFluentValidationPropertyValidator(metadata, context, description, validator));
            });

            // This header cannot be removed using web.config --> http://www.codeproject.com/Tips/785867/ASP-NET-MVC-Remove-IIS-Header-Bloat
            MvcHandler.DisableMvcResponseHeader = true;

            var configurationService = container.GetInstance<IConfigurationService>();
            var webConfiguration = configurationService.Get<CandidateWebConfiguration>();
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = webConfiguration.AppInsightsInstrumentationKey;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Server.HandleError<ErrorController>(((MvcApplication) sender).Context);
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            // This header cannot be removed using web.config --> http://www.codeproject.com/Tips/785867/ASP-NET-MVC-Remove-IIS-Header-Bloat
            var application = sender as HttpApplication;
            if (application != null && application.Context != null)
            {
                application.Context.Response.Headers.Remove("Server");
            }
        }
    }
}