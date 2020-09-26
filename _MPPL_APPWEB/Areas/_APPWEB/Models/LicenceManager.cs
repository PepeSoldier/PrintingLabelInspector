using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Models
{
    public static class LicenceManager
    {
        public static string MtngLicence = "08CDU-K6DSQ-MLDS90-DTA0E";
        public static string MtngLicence1 = "12CDU-K6DSQ-MLDS90-DTA0E";

        public static bool CheckMeetingAccess()
        {
            if (Properties.Settings.Default.MTNG_Lic == MtngLicence)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckMeetingLicence()
        {
            if (Properties.Settings.Default.MTNG_Lic == MtngLicence)
            {
                return true;
            }
            else
            {
                throw new Exception(message: "Nie masz dostępu do modułu MEETING");
            }
        }
    }
}