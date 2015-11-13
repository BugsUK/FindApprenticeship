using System.Web.Optimization;

namespace SFA.Apprenticeships.Web.Recruit
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            Common.BundleConfig.RegisterBundles(bundles);

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                "~/Content/_assets/js/nas/dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Content/_assets/js/vendor/jquery.validate.js",
                "~/Content/_assets/js/vendor/jquery.validate.unobtrusive.custom.js",
                "~/Content/_assets/js/vendor/select2.js"));
        }
    }
}
        