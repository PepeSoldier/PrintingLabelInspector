using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _MPPL_WEB_START.Areas._APPWEB.Models.Clients
{
    public class AppClientDev : AppClientApbstract
    {
        public AppClientDev() : base()
        {
            ac = new MenuAccessCode[] { MenuAccessCode.FULL_ACCESS };
        }
        
        public override void Configure_LABELINSP()
        {
            SettingsLABELINSP.Test = true;   
        }
    }
}