﻿using System.Web;
using System.Web.Optimization;

namespace Prototypes
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery", "//cdnjs.cloudflare.com/ajax/libs/jquery/1.11.1/jquery.min.js").Include(
                "~/Content/_assets/js/vendor/jquery-1.11.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/fastclick", "//cdnjs.cloudflare.com/ajax/libs/fastclick/1.0.6/fastclick.min.js").Include(
                "~/Content/_assets/js/vendor/fastclick-1.0.6.js"));

            bundles.Add(new ScriptBundle("~/bundles/underscore", "//cdnjs.cloudflare.com/ajax/libs/underscore.js/1.7.0/underscore-min.js").Include(
                "~/Content/_assets/js/vendor/underscore-1.7.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/joyride").Include(
                "~/Content/_assets/js/vendor/jquery.joyride.js"));

            bundles.Add(new ScriptBundle("~/bundles/cookie", "//cdnjs.cloudflare.com/ajax/libs/jquery-cookie/1.4.1/jquery.cookie.js").Include(
                "~/Content/_assets/js/vendor/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/nas").Include(
                "~/Content/_assets/js/nas/validationscripts.js",
                "~/Content/_assets/js/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Content/_assets/js/vendor/jquery.validate.js",
                "~/Content/_assets/js/vendor/jquery.validate.unobtrusive.custom.js",
                "~/Content/_assets/js/vendor/chosen.jquery.js"));
        }
    }
}
