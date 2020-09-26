using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Clients
{
    public class AppClientWRP : AppClientApbstract
    {
        public AppClientWRP() : base()
        {
            ONEPROD_RTV_Licence = new DateTime(2020, 1, 30);

            ac = new MenuAccessCode[] {
                    MenuAccessCode.COMMON_ACCESS,
                    MenuAccessCode.ONEPROD_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_DASHBOARD_ACCESS,
                    MenuAccessCode.ONEPROD_MES_ACCESS,
                    //MenuAccessCode.ONEPROD_MES_TRACE_ACCESS,
                    MenuAccessCode.ONEPROD_RTV_ACCESS,
                    //MenuAccessCode.ONEPROD_WMS_ACCESS,
                    MenuAccessCode.ONEPROD_OEE_ENERGY_ACCESS
                };
        }

        public override void Configure_ONEPROD()
        {
            SettingsONEPROD.ReportOnlineAllowedMachinesIDs = new int[] { 0 };   
            SettingsONEPROD.ReportOnlinePillHeigh = 29;
            SettingsONEPROD.MesWorkplaceVerifyIP = true;
            SettingsONEPROD.MediaEnabled = true;
            SettingsONEPROD.TraceabilityEnabled = false;
            SettingsONEPROD.Translate_SCRAP = "BRAK";
            SettingsONEPROD.Translate_Scrap = "Brak";
        }
    }
}