using _MPPL_WEB_START.Areas._APPWEB.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Clients
{
    public abstract class AppClientApbstract
    {
        public AppClientApbstract()
        {
            Configure_ONEPROD();
            Configure_iLOGIS();
        }

        //-----------------------------------------------------ACCESS-CONTROL
        protected MenuAccessCode[] ac;
        public MenuAccessCode[] AccessCodes { get; }
        public DateTime ONEPROD_RTV_Licence = new DateTime(2099, 1, 1);

        public bool hasAccess(MenuAccessCode accessCode)
        {
            if ((accessCode == 0 || (ac.Contains(accessCode) || ac.Contains(MenuAccessCode.FULL_ACCESS))))
            {
                return true;
            }
            return false;
        }

        //--------------------------------------------------------ONEPROD-SETTINGS
        public SettingsONEPROD SettingsONEPROD = new SettingsONEPROD();

        public virtual void Configure_ONEPROD()
        {
        }

        //--------------------------------------------------------ILOGIS-SETTINGS
        public SettingsILOGIS SettingsILOGIS = new SettingsILOGIS();

        public virtual void Configure_iLOGIS()
        {
        }
    }
}