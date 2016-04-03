using System.Web.Optimization;

namespace AjaxifiedPaging
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", "~/Scripts/bootstrap.js", "~/Scripts/mustache.js", "~/Scripts/mustache-wax.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css", "~/Content/bootstrap.css"));
        }
    }
}
