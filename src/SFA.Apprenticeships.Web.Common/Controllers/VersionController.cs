namespace SFA.Apprenticeships.Web.Common.Controllers
{
    using System.Diagnostics;
    using System.Reflection;
    using System.Web.Http;
    using ViewModels;

    public class VersionController : ApiController
    {
        // GET api/<controller>   : url to use => api/version
        public VersionViewModel Get()
        {
            var assembly = GetWebEntryAssembly();
            return new VersionViewModel
            {
                AssemblyVersion = assembly.GetName().Version.ToString(),
                PackageVersion = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion
            };
        }

        private static Assembly GetWebEntryAssembly()
        {
            if (System.Web.HttpContext.Current == null ||
                System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                return null;
            }

            var type = System.Web.HttpContext.Current.ApplicationInstance.GetType();
            while (type != null && type.Namespace == "ASP")
            {
                type = type.BaseType;
            }

            return type == null ? null : type.Assembly;
        }
    }
}