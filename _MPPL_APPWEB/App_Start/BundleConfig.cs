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
            ACTIONPLANS_MODULE_Scripts_AND_Styles(bundles);
            ONEPROD_MODULE_Scripts_AND_Styles(bundles);
            PRD_MODULE_Scripts_AND_Styles(bundles);
            PFEP_MODULE_Scripts_AND_Styles(bundles);
            iLOGIS_MODULE_Scripts_AND_Styles(bundles);
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
        private static void ACTIONPLANS_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            //-----------------MODULE-ACTION-PLANS--------------------------------------
            //--------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/AP_Styles").Include(
                "~/_ClientAppJS/AP/AP_Style.min.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/AP_Scripts").Include(
                "~/_ClientAppJS/AP/Action/browse.js",
                "~/_ClientAppJS/AP/Action/Show.js",
                "~/_ClientAppJS/AP/ActionActivity/ActionActivity.js",
                "~/_ClientAppJS/AP/Dashboard/dashboard.js"
            ));
        }
        private static void ONEPROD_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            //-----------------MODULE-ONEPROD-------------------------------------------
            //--------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/ONEPROD_Styles").Include(
                "~/Areas/ONEPROD/Views/ONEPROD_Style.min.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/ONEPROD_Scripts")
                .IncludeDirectory("~/Areas/ONEPROD/Views/APS/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/ClientOrder/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/Configuration/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/ConfigurationAPS/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/ConfigurationENERGY/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/ConfigurationMES/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/ConfigurationOEE/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/ConfigurationWMS/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/JobList/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/MES/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/OEECreateReport/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/OEEReportOnline/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/Quality/", "*.js", true)
                .IncludeDirectory("~/Areas/ONEPROD/Views/RTV/", "*.js", true)
                .Include(                
                //"~/Areas/ONEPROD/Views/APS/Calculation.js",
                //"~/Areas/ONEPROD/Views/APS/GanttChart.js",
                //"~/Areas/ONEPROD/Views/ClientOrder/ClientOrderGrid.js",
                //"~/Areas/ONEPROD/Views/JobList/JobList.js",
                "~/Areas/ONEPROD/Views/Home/WorkingHoursPreview.js",
                "~/Areas/ONEPROD/Views/Home/WeekPicker.js",
                "~/Areas/ONEPROD/Views/Quality/JobLabelCheck.js",
                "~/Areas/ONEPROD/Views/BOM/BOM.js",

                //"~/Areas/ONEPROD/Views/MES/Workplace.js",
                //"~/Areas/ONEPROD/Views/MES/WorkplaceBuffer.js",
                //"~/Areas/ONEPROD/Views/MES/WorkplaceTraceability.js",
                //"~/Areas/ONEPROD/Views/MES/WorkplaceWorkorder.js",

                "~/Areas/ONEPROD/Views/OEEDashboard/Dashboard.js",
                "~/Areas/ONEPROD/Views/OEEDashboard/MachineDetails_OEEGauges.js",
                "~/Areas/ONEPROD/Views/OEEBrowse/OeeBrowseGrid.js"
                //"~/Areas/ONEPROD/Views/OEEReportOnline/ReportOnline.js",
                //"~/Areas/ONEPROD/Views/OEEReportOnline/ReportOnline.ReasonSelector.js",
                //"~/Areas/ONEPROD/Views/OEEReportOnline/StopSplit.js",
                //"~/Areas/ONEPROD/Views/OEEReportOnline/ChangeDeclarationDate.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Index_HeaderStepIndicator.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step3_ItemAutocompleteOEE.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step2_OperatorsGrid.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step2_EmployeeAutocomplete.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step3_ANCautocomplete.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step3_ProductionDataGrid.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_ReasonsManager.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_Stop_GridHelper.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_StopBreakdownGrid.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_StopChangeOver.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_StopPerformance.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_StopPlannedGrid.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_StopUnplannedChangeOver.js",
                //"~/Areas/ONEPROD/Views/OEECreateReport/Step4_StopUnplannedGrid.js",
                
                //"~/Areas/ONEPROD/Views/RTV/MachineDetails.js",

                //"~/Areas/ONEPROD/Views/Configuration/Item.js",
                //"~/Areas/ONEPROD/Views/Configuration/ItemGroup.js",
                //"~/Areas/ONEPROD/Views/Configuration/ItemGroupCycleTimePopup.js",
                //"~/Areas/ONEPROD/Views/Configuration/ItemGroupToolPopup.js",
                //"~/Areas/ONEPROD/Views/Configuration/Process.js",
                //"~/Areas/ONEPROD/Views/Configuration/Resource.js",
                //"~/Areas/ONEPROD/Views/Configuration/ResourceGroup.js",
                //"~/Areas/ONEPROD/Views/ConfigurationAPS/ToolMachine.js",
                //"~/Areas/ONEPROD/Views/ConfigurationMES/Workplace.js",
                //"~/Areas/ONEPROD/Views/ConfigurationOEE/ReasonGrid.js",
                //"~/Areas/ONEPROD/Views/ConfigurationOEE/ResourceTarget.js",
                //"~/Areas/ONEPROD/Views/ConfigurationWMS/Box.js",
                //"~/Areas/ONEPROD/Views/ConfigurationWMS/BoxItemGroupPopup.js",
                //"~/Areas/ONEPROD/Views/ConfigurationEnergy/EnergyMeter.js",
                //"~/Areas/ONEPROD/Views/ConfigurationEnergy/EnergyCost.js"
            ));
        }
        private static void PRD_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            //-----------------MODULE-PRD-----------------------------------------------
            //--------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/PRD_Styles").Include(
                "~/Areas/PRD/Views/PRD_style.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/PRD_Scripts").Include(
                "~/Areas/PRD/Views/PSI/Analyze.js",
                "~/Areas/PRD/Views/PSI/ReasonSelector.js",
                "~/Areas/PRD/Views/Schedule/ScheduleGrid.js",
                "~/Areas/PRD/Views/Schedule/StautsGrid.js"
            ));
        }
        private static void PFEP_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            //-----------------MODULE-PFEP----------------------------------------------
            //--------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/PFEP_Styles").Include(
                "~/_ClientAppJS/PFEP/PFEP_style.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/PFEP_Scripts").Include(
                "~/_ClientAppJS/PFEP/Print/Print.js",
                "~/_ClientAppJS/PFEP/Print/PrintMatrix.js"
            ));
        }
        private static void iLOGIS_MODULE_Scripts_AND_Styles(BundleCollection bundles)
        {
            //-----------------MODULE-iLOGIS----------------------------------------------
            //--------------------------------------------------------------------------
            bundles.Add(new StyleBundle("~/Content/iLOGIS_Styles").Include(
                "~/Areas/iLOGIS/Views/iLOGIS_Style.min.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/iLOGIS_Scripts")
                .IncludeDirectory("~/Areas/iLOGIS/Views/Config/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/PFEP/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/Delivery/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/DeliveryList/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/DeliveryListLineFeed/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/PickingList/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/PickingListItem/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/StockUnit/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/Movement/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/Warehouse/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/WhDoc/", "*.js", true)
                .IncludeDirectory("~/Areas/iLOGIS/Views/WMS/", "*.js", true)
                .Include(
                    "~/Areas/iLOGIS/Views/iLOGIS_Script.bundle.js"
                )
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
