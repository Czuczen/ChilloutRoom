using System.Web.Optimization;

namespace CzuczenLand.Web;

public static class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        bundles.IgnoreList.Clear();

        bundles.Add(
            new StyleBundle("~/Bundles/account-vendor/css")
                .Include("~/fonts/roboto/roboto.css", new CssRewriteUrlTransform())
                .Include("~/fonts/material-icons/materialicons.css", new CssRewriteUrlTransform())
                .Include("~/lib/bootstrap/dist/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/lib/toastr/toastr.css", new CssRewriteUrlTransform())
                .Include("~/lib/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/lib/Waves/dist/waves.css", new CssRewriteUrlTransform())
                .Include("~/lib/animate.css/animate.css", new CssRewriteUrlTransform())
                .Include("~/css/materialize.css", new CssRewriteUrlTransform())
                .Include("~/css/style.css", new CssRewriteUrlTransform())
                .Include("~/Views/Account/_Layout.css", new CssRewriteUrlTransform())
        );

        bundles.Add(
            new ScriptBundle("~/Bundles/account-vendor/js/bottom")
                .Include(
                    "~/lib/json2/json2.js",
                    "~/lib/jquery/dist/jquery.js",
                    "~/lib/bootstrap/dist/js/bootstrap.js",
                    "~/lib/moment/min/moment-with-locales.js",
                    "~/lib/jquery-validation/dist/jquery.validate.js",
                    "~/lib/blockUI/jquery.blockUI.js",
                    "~/lib/toastr/toastr.js",
                    "~/lib/sweetalert/dist/sweetalert.min.js",
                    "~/lib/spin.js/spin.js",
                    "~/lib/spin.js/jquery.spin.js",
                    "~/lib/Waves/dist/waves.js",
                    "~/Abp/Framework/scripts/abp.js",
                    "~/Abp/Framework/scripts/libs/abp.jquery.js",
                    "~/Abp/Framework/scripts/libs/abp.toastr.js",
                    "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                    "~/Abp/Framework/scripts/libs/abp.spin.js",
                    "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                    "~/js/admin.js",
                    "~/js/main.js"                   
                )
        );

        //VENDOR RESOURCES

        //~/Bundles/vendor/css
        bundles.Add(
            new StyleBundle("~/Bundles/vendor/css")
                .Include("~/fonts/roboto/roboto.css", new CssRewriteUrlTransform())
                .Include("~/fonts/material-icons/materialicons.css", new CssRewriteUrlTransform())
                .Include("~/lib/bootstrap/dist/css/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/lib/bootstrap-select/dist/css/bootstrap-select.css", new CssRewriteUrlTransform())
                .Include("~/lib/toastr/toastr.css", new CssRewriteUrlTransform())
                .Include("~/lib/font-awesome/css/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/lib/Waves/dist/waves.css", new CssRewriteUrlTransform())
                .Include("~/lib/animate.css/animate.css", new CssRewriteUrlTransform())
                .Include("~/css/materialize.css", new CssRewriteUrlTransform())
                .Include("~/css/style.css", new CssRewriteUrlTransform())
                .Include("~/css/themes/all-themes.css", new CssRewriteUrlTransform())
                .Include("~/Views/Shared/_Layout.css", new CssRewriteUrlTransform())
                .Include("~/css/DataTables/css/jquery.dataTables.min.css", new CssRewriteUrlTransform())
                .Include("~/css/DataTables/css/responsive.dataTables.min.css", new CssRewriteUrlTransform())
                .Include("~/css/DataTables/css/buttons.dataTables.min.css", new CssRewriteUrlTransform())
                .Include("~/css/select2.min.css", new CssRewriteUrlTransform())
                
        );

        //~/Bundles/vendor/bottom (Included in the bottom for fast page load)
        bundles.Add(
            new ScriptBundle("~/Bundles/vendor/js/bottom")
                .Include(
                    "~/lib/json2/json2.js",
                    "~/lib/jquery/dist/jquery.js",
                    "~/lib/bootstrap/dist/js/bootstrap.js",
                    "~/lib/moment/min/moment-with-locales.js",
                    "~/lib/jquery-validation/dist/jquery.validate.js",
                    "~/lib/blockUI/jquery.blockUI.js",
                    "~/lib/toastr/toastr.js",
                    "~/lib/sweetalert/dist/sweetalert.min.js",
                    "~/lib/spin.js/spin.js",
                    "~/lib/spin.js/jquery.spin.js",
                    "~/lib/bootstrap-select/dist/js/bootstrap-select.js",
                    "~/lib/jquery-slimscroll/jquery.slimscroll.js",
                    "~/lib/Waves/dist/waves.js",
                    "~/lib/push.js/push.js",
                    "~/lib/jquery.serializejson/jquery.serializejson.js",
                    "~/Abp/Framework/scripts/abp.js",
                    "~/Abp/Framework/scripts/libs/abp.jquery.js",
                    "~/Abp/Framework/scripts/libs/abp.toastr.js",
                    "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                    "~/Abp/Framework/scripts/libs/abp.spin.js",
                    "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",
                    "~/lib/DataTables/jquery.dataTables.min.js",
                    "~/lib/DataTables/dataTables.responsive.min.js",
                    "~/lib/DataTables/dataTables.buttons.js",
                    "~/lib/DataTables/dataTables.select.js",
                    "~/lib/DataTables/buttons.colVis.js",
                    "~/lib/select2.min.js",
                    "~/lib/select2-pl.js",
                    "~/js/admin.js",
                    "~/js/main.js",
                    "~/Scripts/jquery.signalR-2.4.3.js",
                    "~/Views/Shared/_Layout.js"
                )
        );

        // //Home-Index Bundles
        // bundles.Add(
        //     new ScriptBundle("~/Bundles/home-index")
        //         .Include(
        //             // "~/Views/Home/Index.js"
        //         )
        // );

        //APPLICATION RESOURCES

        //~/Bundles/css
        bundles.Add(
            new StyleBundle("~/Bundles/css")
                .Include("~/css/main.css")
        );
    }
}