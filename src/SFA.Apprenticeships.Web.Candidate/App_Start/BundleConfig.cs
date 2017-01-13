using System.Web.Optimization;

namespace SFA.Apprenticeships.Web.Candidate
{
    using System.Collections.Generic;

    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles. For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            Common.BundleConfig.RegisterBundles(bundles);

            bundles.Remove(bundles.GetBundleFor("~/bundles/nas"));
            bundles.Add(new ScriptBundle("~/bundles/nas").Include(
                "~/Content/_assets/js/nas/validationscripts.js",
                "~/Content/_assets/js/nas/headerContext.js",
                "~/Content/_assets/js/nas/webTrendsInlineTrack.js"));

            bundles.Add(new ScriptBundle("~/bundles/nascript").Include(
                  "~/Content/_assets/js/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Content/_assets/js/vendor/jquery.validate.js",
                "~/Content/_assets/js/vendor/jquery.validate.unobtrusive.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Content/_assets/js/vendor/knockout-3.1.0.js",
                "~/Content/_assets/js/vendor/knockout.mapping-latest.js",
                "~/Content/_assets/js/vendor/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/passwordstrength").Include(
                "~/Content/_assets/js/vendor/zxcvbn-async.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/application").Include(
                "~/Content/_assets/js/nas/application/applicationform.js",
                "~/Content/_assets/js/nas/application/dirtyFormDialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/account").Include(
                "~/Content/_assets/js/vendor/jquery-ui-1.10.4.custom.min.js",
                "~/Content/_assets/js/nas/lookupService.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/locationsearch").Include(
                "~/Content/_assets/js/vendor/jquery-ui-1.10.4.custom.min.js",
                "~/Content/_assets/js/nas/locationAutocomplete.js",
                "~/Content/_assets/js/nas/refineSearch.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/results").Include(
                "~/Content/_assets/js/nas/resultsSearch.js",
                "~/Content/_assets/js/nas/apprenticeships/saveVacancy.js",
                "~/Content/_assets/js/vendor/jquery.lazy-load-google-maps.js",
                "~/Content/_assets/js/nas/resultsMap.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/details").Include(
                "~/Content/_assets/js/nas/apprenticeships/saveVacancy.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/geoLocater").Include(
                "~/Content/_assets/js/nas/geoLocater.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas/search").Include(
                "~/Content/_assets/js/nas/searchTour.js"));
        }
    }
}
