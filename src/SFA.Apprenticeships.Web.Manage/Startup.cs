using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SFA.Apprenticeships.Web.Manage.Startup))]
namespace SFA.Apprenticeships.Web.Manage
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthenticationConfig.RegisterProvider(app);
        }
    }
}
