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
            Configure_LABELINSP();
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
        public SettingsLABELINSP SettingsLABELINSP = new SettingsLABELINSP();

        public virtual void Configure_LABELINSP()
        {
        }
    }
}