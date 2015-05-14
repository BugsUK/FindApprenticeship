namespace SFA.Apprenticeships.Web.Common.IoC
{
    using Services;
    using StructureMap.Configuration.DSL;
    using StructureMap.Web;

    public class WebCommonRegistry : Registry
    {
        public WebCommonRegistry()
        {
            For<IAuthenticationTicketService>().HttpContextScoped().Use<AuthenticationTicketService>();
        }
    }
}