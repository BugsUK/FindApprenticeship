namespace SFA.DAS.RAA.Api
{
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using Attributes;
    using FluentValidation.WebApi;
    using Handlers;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableSystemDiagnosticsTracing();
            config.Services.Add(typeof(IExceptionLogger), new AiExceptionLogger());
            config.Filters.Add(new ValidateModelStateFilter());
            config.Filters.Add(new ExceptionToErrorResponseFilterAttribute());

            // configure FluentValidation model validator provider
            FluentValidationModelValidatorProvider.Configure(config);

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
