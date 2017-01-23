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

            bundles.Add(new ScriptBundle("~/bundles/print").Include(
                "~/Content/_assets/js/print.js"));

            bundles.Add(new ScriptBundle("~/bundles/autosave").Include(
                "~/Content/_assets/js/nas/autosave.js"));

            bundles.Add(new ScriptBundle("~/bundles/vacancyApplications").Include(
                "~/Content/_assets/js/nas/vacancyApplications.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Content/_assets/js/vendor/jquery.validate.js",
                "~/Content/_assets/js/vendor/jquery.validate.unobtrusive.custom.js",
                "~/Content/_assets/js/vendor/select2.js",
                "~/Content/_assets/js/vendor/fastclick.js"));

            bundles.Add(new ScriptBundle("~/bundles/location").Include(
                "~/Content/_assets/js/locationAutocomplete.js",
                "~/Content/_assets/js/lookupService.js",
                "~/Content/_assets/js/inplaceediting.js",
                "~/Content/_assets/js/nas/multilocation.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Content/_assets/js/vendor/knockout-3.1.0.js",
                "~/Content/_assets/js/vendor/knockout.mapping-latest.js",
                "~/Content/_assets/js/vendor/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/webtrends").Include(                
                "~/Content/_assets/js/webtrends/webtrends.load.js"));   

            bundles.Add(new ScriptBundle("~/bundles/reports").Include(
                "~/Content/_assets/js/nas/reports.js"));

            bundles.Add(new ScriptBundle("~/bundles/candidates").Include(
                "~/Content/_assets/js/nas/candidates.js"));

            bundles.Add(new ScriptBundle("~/bundles/basicVacancyDetails").Include(
                "~/Content/_assets/js/nas/basicVacancyDetails.js"));
        }
    }
}
        