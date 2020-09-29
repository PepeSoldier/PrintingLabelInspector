using System.Web;
using System.Web.Optimization;

namespace _MPPL_WEB_START
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            EXTERNAL_Scripts_AND_Styles(bundles);
            COMMON_MPPL_MODULE_Scripts_AND_Styles(bundles);
            LABELINSP_MODULE_Scripts_AND_Styles(bundles);
            CORE_MODULE_SIGN_Scripts_AND_Styles(bundles);
            BundleTable.EnableOptimizations = true;
        }

        private static void COMMON_MPPL_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/_ClientAppJS/ExternalLibraries/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/AppScripts")
                            //.IncludeDirectory("~/_ClientAppJS/_APPWEB/Common/", "*.js", true)
                            .Include(
                            "~/_ClientAppJS/ExternalLibraries/SammyJS/SammyJS.js",
                            "~/_ClientAppJS/ExternalLibraries/OnScan/onscan.min.js",
                            "~/_ClientAppJS/MyScripts.js",
                            "~/_ClientAppJS/_APPWEB/App.js",
                            //"~/_ClientAppJS/_APPWEB/AppTS.js",
                            "~/_ClientAppJS/_APPWEB/Common/BtnAnimation.Clip.js",
                            "~/_ClientAppJS/_APPWEB/Common/GridDefault.js",
                            "~/_ClientAppJS/_APPWEB/Common/GridHelper.js",
                            "~/_ClientAppJS/_APPWEB/Common/JsonHelper.js",
                            "~/_ClientAppJS/_APPWEB/Common/LeftMenu.js",
                            "~/_ClientAppJS/_APPWEB/Common/SidebarWrapper.js",
                            "~/_ClientAppJS/_APPWEB/Common/VersionController.js",
                            "~/_ClientAppJS/_APPWEB/Routing/MapRoute.js",
                            "~/_ClientAppJS/_APPWEB/Routing/Routing.js",
                            "~/_ClientAppJS/_APPWEB/Alert/Alert.js",
                            "~/_ClientAppJS/_APPWEB/Attachment/Attachment.js",
                            "~/_ClientAppJS/_APPWEB/PopupWindow/PopupWindow.js",
                            "~/_ClientAppJS/_APPWEB/PopupWindow/Window.js",
                            "~/_ClientAppJS/_APPWEB/Keypad/Keypad.js",
                            "~/Areas/CORE/Views/Printer/PrinterGrid.js"
                        )); ;
            bundles.Add(new StyleBundle("~/Content/AppStyles").Include(
                "~/Content/site.css",
                "~/_ClientAppJS/_APPWEB/App.min.css",
                "~/_ClientAppJS/_APPWEB/Keypad/Keypad.min.css"
            ));
        }
        
        private static void LABELINSP_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            ////-----------------MODULE-LABELINSP----------------------------------------------
            ////--------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/LABELINSP_Styles").Include(
                "~/Areas/LABELINSP/Views/LABELINSP_Style.min.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/LABELINSP_Scripts")
                .IncludeDirectory("~/Areas/LABELINSP/Views/", "*.js", true)
            );
        }
        private static void EXTERNAL_Scripts_AND_Styles(BundleCollection bundles)
        {
            //-----------------EXTERNAL-------------------------------------------------
            //--------------------------------------------------------------------------
            bundles.Add(new ScriptBundle("~/bundles/ExternalScripts").Include(
                "~/_ClientAppJS/ExternalLibraries/jQuery/jquery-3.1.0.min.js",
                "~/_ClientAppJS/ExternalLibraries/jQuery/jquery-ui-1.12.1.min.js",
                //"~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap.js",
                "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap.bundle.min.js",
                "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap-checkbox.min.js",
                "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap-select.js",
                "~/_ClientAppJS/ExternalLibraries/bootstrap/bootbox.min.js",
                "~/_ClientAppJS/ExternalLibraries/respond.js",
                "~/_ClientAppJS/ExternalLibraries/moment.js",
                "~/_ClientAppJS/ExternalLibraries/custom.js",
                //"~/_ClientAppJS/ExternalLibraries/popper.js",
                //"~/_ClientAppJS/ExternalLibraries/popper-utils.js",
                "~/_ClientAppJS/ExternalLibraries/jQuery/jquery.datetimepicker.full.min.js",
                "~/_ClientAppJS/ExternalLibraries/jQuery/jquery.metisMenu.js",
                "~/_ClientAppJS/ExternalLibraries/fancybox-master/dist/jquery.fancybox.min.js",
                "~/_ClientAppJS/ExternalLibraries/html2canvas/html2canvas.js",
                "~/_ClientAppJS/ExternalLibraries/ChartJS/Chart.js",
                "~/_ClientAppJS/ExternalLibraries/ChartJS/Chart.PieceLabel.js",
                "~/_ClientAppJS/ExternalLibraries/ChartJS/chartjs-plugin-datalabels.min.js",
                "~/_ClientAppJS/ExternalLibraries/ChartJS/chartjs-plugin-trendline.js",
                "~/_ClientAppJS/ExternalLibraries/gridmvc/gridmvc.custom.js",
                "~/_ClientAppJS/ExternalLibraries/dhtmlxGantt/dhtmlxgantt.js",
                "~/_ClientAppJS/ExternalLibraries/dhtmlxGantt/ext/dhtmlxgantt_tooltip.js",
                "~/_ClientAppJS/ExternalLibraries/dhtmlxGantt/ext/dhtmlxgantt_marker.js",
                "~/_ClientAppJS/ExternalLibraries/jquery.bootgrid/jquery.bootgrid.min.js",
                "~/_ClientAppJS/ExternalLibraries/jsgrid-1.5.3/jsgrid.js",
                "~/_ClientAppJS/ExternalLibraries/colorPicker/js/colorpicker.js",
                "~/_ClientAppJS/ExternalLibraries/colorPicker/js/eye.js",
                "~/_ClientAppJS/ExternalLibraries/colorPicker/js/utils.js",
                "~/_ClientAppJS/ExternalLibraries/Sortable/sortable.js",
                "~/_ClientAppJS/ExternalLibraries/hammer.min.js",
                "~/_ClientAppJS/ExternalLibraries/MustacheJS/mustache.js",
                "~/_ClientAppJS/ExternalLibraries/MustacheJS/mostache.js"
            ));

            bundles.Add(new StyleBundle("~/_ClientAppJS/ExternalLibraries/ExternalStylesBundle").Include(
               "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap.min.css",
               "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap-grid.min.css",
               "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap-reboot.min.css",
               "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap-theme.min.css",
               "~/_ClientAppJS/ExternalLibraries/bootstrap/bootstrap-select.css",
               "~/_ClientAppJS/ExternalLibraries/jQuery/jquery-ui.structure.min.css",
               "~/_ClientAppJS/ExternalLibraries/jQuery/jquery-ui.theme.min.css",
               "~/_ClientAppJS/ExternalLibraries/jQuery/jquery.datetimepicker.min.css",
               "~/_ClientAppJS/ExternalLibraries/fancybox-master/dist/jquery.fancybox.min.css",
               "~/_ClientAppJS/ExternalLibraries/fancycheckbox/fancycheckbox.css",
               "~/_ClientAppJS/ExternalLibraries/dhtmlxGantt/dhtmlxgantt.css",
               "~/_ClientAppJS/ExternalLibraries/jquery.bootgrid/jquery.bootgrid.min.css",
               "~/_ClientAppJS/ExternalLibraries/jsgrid-1.5.3/jsgrid.min.css",
               "~/_ClientAppJS/ExternalLibraries/jsgrid-1.5.3/jsgrid-theme.min.css",
               //"~/_ClientAppJS/ExternalLibraries/colorPicker/css/colorpicker.css"
               "~/_ClientAppJS/ExternalLibraries/ExternalStyles.min.css"
           )
           .Include("~/_ClientAppJS/ExternalLibraries/font-awesome5/css/all.min.css", new CssRewriteUrlTransform())
           );
        }
        private static void CORE_MODULE_SIGN_Scripts_AND_Styles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/CORE_SIGN_Scripts")
                .IncludeDirectory("~/Areas/CORE/Views/Sign/", "*.js", true)
            );
        }
    }
}
