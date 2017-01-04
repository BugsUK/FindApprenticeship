namespace SFA.Apprenticeships.Web.Common
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery"
                , "//cdnjs.cloudflare.com/ajax/libs/jquery/1.11.1/jquery.min.js"
                ).Include(
                    "~/Content/_assets/js/vendor/jquery-1.11.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick",
                "//cdnjs.cloudflare.com/ajax/libs/fastclick/1.0.6/fastclick.min.js").Include(
                    "~/Content/_assets/js/vendor/fastclick-1.0.6.js"));

            bundles.Add(new ScriptBundle("~/bundles/underscore",
                "//cdnjs.cloudflare.com/ajax/libs/underscore.js/1.7.0/underscore-min.js").Include(
                    "~/Content/_assets/js/vendor/underscore-1.7.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/joyride").Include(
                "~/Content/_assets/js/vendor/jquery.joyride.js"));

            bundles.Add(new ScriptBundle("~/bundles/cookie",
                "//cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.js").Include(
                    "~/Content/_assets/js/vendor/jquery.cookie.js"));            

            // TODO: split in different bundles?
            bundles.Add(new ScriptBundle("~/bundles/nas").Include(
                "~/Content/_assets/js/nas/validationscripts.js",
                "~/Content/_assets/js/inplaceediting.js",
                "~/Content/_assets/js/interactions.js",
                "~/Content/_assets/js/jobtitles.js",
                "~/Content/_assets/js/jquery.pwstrength.js",
                "~/Content/_assets/js/maps.js",
                "~/Content/_assets/js/old-browsers.js",
                "~/Content/_assets/js/sticky.js",
                "~/Content/_assets/js/vendor/jquery-ui-1.10.4.custom.min.js",
                "~/Content/_assets/js/nas/lookupService.js",
                "~/Content/_assets/js/nas/locationAutocomplete.js",
                "~/Content/_assets/js/nas/refineSearch.js"));
            
            bundles.Add(new StyleBundle("~/Content/_assets/styles/not-ie8")
                //.Include("~/Content/_assets/css/main.css")
                .Include("~/Content/_assets/css/govuk-template.css", new CssRewriteUrlTransform())
                .Include("~/Content/_assets/css/styles.css")
                .Include("~/Content/_assets/css/fonts.css"));

            bundles.Add(new StyleBundle("~/Content/_assets/styles/ie8")
                .Include("~/Content/_assets/css/main-ie8.css")
                .Include("~/Content/_assets/css/fonts-ie8.css"));

            // Developers are must unlikely to be interested in the un-mimified version of this
            // TODO: Find and add approved CDN
            bundles.Add(new StyleBundle("~/bundles/font-awesome")
                .Include("~/Content/_assets/css/font-awesome.css", new CssRewriteUrlTransform()));
        }
    }
}