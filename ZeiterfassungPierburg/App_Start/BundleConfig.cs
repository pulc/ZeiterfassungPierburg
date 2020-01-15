using System.Web;
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
