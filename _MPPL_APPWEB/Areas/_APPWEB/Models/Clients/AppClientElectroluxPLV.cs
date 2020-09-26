using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Clients
{
    public class AppClientElectroluxPLV : AppClientApbstract
    {
        public AppClientElectroluxPLV() : base()
        {
            ac = new MenuAccessCode[] {
                    MenuAccessCode.COMMON_ACCESS,
                    MenuAccessCode.PFEP_ACCESS,
                    MenuAccessCode.PFEP_LINEFEED_ACCESS,
                    MenuAccessCode.PRD_ACCESS,
                    MenuAccessCode.ONEPROD_ACCESS,
                    MenuAccessCode.ONEPROD_APS_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_COMPARE_ACCESS,
                    MenuAccessCode.ONEPROD_MES_ACCESS,
                    MenuAccessCode.ONEPROD_MES_TRACE_ACCESS,
                    MenuAccessCode.ONEPROD_RTV_ACCESS,
                    MenuAccessCode.ONEPROD_WMS_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_REPORTS_ACCESS,
                    MenuAccessCode.ONEPROD_QUALITY_ACCESS,
                    MenuAccessCode.ILOGIS_ACCESS,
                    //MenuAccessCode.ILOGIS_DASHBOARD_ACCESS,
                    MenuAccessCode.ILOGIS_PFEP_ACCESS,
                    MenuAccessCode.ILOGIS_PICKLIST_ACCESS,
                    MenuAccessCode.ILOGIS_DELIVERYLIST_ACCESS,
                    MenuAccessCode.ILOGIS_JOBLIST_ACCESS,
                    //MenuAccessCode.ILOGIS_WHDOC_ACCESS,
                    MenuAccessCode.ILOGIS_CONFIG,
                    MenuAccessCode.ILOGIS_CONFIG_RESOURCE,
                    MenuAccessCode.ILOGIS_CONFIG_WORKSTATIONS,
                    MenuAccessCode.ILOGIS_CONFIG_ITEMS,
                    MenuAccessCode.ILOGIS_CONFIG_PACKAGES,
                    MenuAccessCode.ILOGIS_CONFIG_PACKAGEITEM,
                    MenuAccessCode.ILOGIS_CONFIG_WORKSTATIONITEM,
                    MenuAccessCode.ILOGIS_CONFIG_TRANSPORTER,
                    MenuAccessCode.ILOGIS_CONFIG_AUTOMATICRULE,
                    MenuAccessCode.ILOGIS_CONFIG_AUTOMATICRULEPACKAGE,
                    MenuAccessCode.ILOGIS_CONFIG_PFEPDATA,
                    MenuAccessCode.ILOGIS_CONFIG_WAREHOUSE,
                    MenuAccessCode.ILOGIS_CONFIG_WAREHOUSELOCATION,
                    MenuAccessCode.ILOGIS_CONFIG_WAREHOUSELOCATIONTYPE,
                    MenuAccessCode.ILOGIS_CONFIG_WAREHOUSELOCATIONSORT,
                    MenuAccessCode.ILOGIS_CONFIG_ITEMCOPY,
                    //MenuAccessCode.ILOGIS_WMS_ACCESS,
                };
        }

        public override void Configure_ONEPROD()
        {
            SettingsONEPROD.ReportOnlineAllowedMachinesIDs = new int[] { 0 };
            SettingsONEPROD.ReportOnlinePillHeigh = 50;
            SettingsONEPROD.ReportOnlinePillFontSize = 40;
            SettingsONEPROD.MesWorkplaceVerifyIP = false;
            SettingsONEPROD.MediaEnabled = false;
            SettingsONEPROD.TraceabilityEnabled = true;
        }

        public override void Configure_iLOGIS()
        {
            SettingsILOGIS.ShowPackagePriceColumn = false;
            SettingsILOGIS.PrintLabel = false;
        }
    }
}