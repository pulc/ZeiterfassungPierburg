﻿using System.Web;
using System.Web.Optimization;

namespace ZeiterfassungPierburg
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));



            bundles.Add(new ScriptBundle("~/bundles/dataTablesPlugins").Include(
    "~/Scripts/datablesPlugins/jquery-3.3.1.js",
    "~/Scripts/datablesPlugins/jquery.dataTables.min.js",
    "~/Scripts/datablesPlugins/dataTables.buttons.min.js",
    "~/Scripts/datablesPlugins/dataTables.buttons.flash.min.js",
    "~/Scripts/datablesPlugins/jszip.min.js",
    "~/Scripts/datablesPlugins/pdfmake.min.js",
    "~/Scripts/datablesPlugins/vfs_fonts.js",
    "~/Scripts/datablesPlugins/buttons.html5.min.js",
    "~/Scripts/datablesPlugins/buttons.print.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));


            bundles.Add(new StyleBundle("~/bundles/tableStyleBundle").Include(
                "~/Content/dataTablebootstrap.min.css",
                "~/Content/dataTables.bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/tablePluginStyleBundle").Include(
    "~/Content/jquery.dataTables.min.css",
    "~/Content/buttons.dataTables.min.css"
    ));


            bundles.Add(new StyleBundle("~/bundles/tableScriptBundle").Include(

"~/Scripts/jquery.dataTables.min.js",
"~/Scripts/dataTables.bootstrap.min.js"
));



            /*
             * 
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                                                    "~/Content/bootstrap.css",
                                                    "~/Content/bootstrap-theme.css",
                                                    "~/Content/site.css",
                                                    "~/Content/dist/css/AdminLTE.css",
                                                    "~/Content/dist/css/skins/_all-skins.min.css"

                                                ));


                  bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate*"));



            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
                        */
        }
    }
}
