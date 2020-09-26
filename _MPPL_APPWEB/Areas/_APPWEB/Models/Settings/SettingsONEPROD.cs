using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Settings
{
    public class SettingsONEPROD
    {
        /// <summary>{ 0 } means all machines allowed </summary>
        public int[] ReportOnlineAllowedMachinesIDs = new int[] { };
        public int ReportOnlinePillHeigh = 29;
        public int ReportOnlinePillFontSize = 26;
        public bool MesWorkplaceVerifyIP = true;
        public bool MediaEnabled = false;
        public bool TraceabilityEnabled = false;

        public string Translate_SCRAP = "SCRAP";
        public string Translate_Scrap = "Scrap";
    }
}